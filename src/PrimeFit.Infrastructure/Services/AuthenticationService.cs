using AutoMapper;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Domain.Specifications.RefreshTokens;
using PrimeFit.Infrastructure.Common.Options;
using PrimeFit.Infrastructure.Data;
using PrimeFit.Infrastructure.Data.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PrimeFit.Infrastructure.Services
{
    internal class AuthenticationService(
         IOptions<JwtOptions> jwtOptions,
          UserManager<ApplicationUser> userManager,
          IUnitOfWork unitOfWork,
          IMapper mapper,
          RoleManager<ApplicationRole> roleManager,
          ICurrentUserService currentUserService,
          AppDbContext dbContext,
          ILogger<AuthenticationService> logger,
          IDateTimeProvider dateTimeProvider,
          IOtpService otpService,
          IOptions<EmailVerificationCodeOptions> emailVerificationCodeOptions) : IAuthenticationService
    {

        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly EmailVerificationCodeOptions _emailOptions = emailVerificationCodeOptions.Value;


        public async Task<ErrorOr<AuthResult>> SignInWithPassword(string email, string password, CancellationToken ct = default)
        {
            var userFromDb = await userManager.Users.Include(au => au.DomainUser)
                                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken: ct);
            if (userFromDb is null)
            {
                logger.LogWarning("Failed login attempt for email: {Email} - User not found", email);
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidCredentials, description: "Invalid Email or password");
            }

            bool isAuthenticated = await userManager.CheckPasswordAsync(userFromDb, password);
            if (!isAuthenticated)
            {
                logger.LogWarning("Failed login attempt for email: {Email} - Invalid password", email);
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidCredentials, description: "Invalid Email or password");
            }

            var result = await AuthenticateAsync(userFromDb);
            if (!result.IsError)
            {
                logger.LogInformation("Successful login for user: {Email}", email);
            }
            return result;
        }

        public async Task<ErrorOr<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken, CancellationToken ct = default)
        {
            var isValidAccessToken = ValidateAccessToken(accessToken, validateLifetime: false);
            if (!isValidAccessToken)
            {
                logger.LogWarning("ReAuthenticate failed - Invalid access token");
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidAccessToken, description: "Invalid access token");
            }

            var jwt = ReadJWT(accessToken);
            if (jwt is null)
            {
                logger.LogWarning("ReAuthenticate failed - Cannot read JWT");
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidAccessToken, description: "Invalid access token");
            }

            var domainUserId = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (domainUserId is null)
            {
                logger.LogWarning("ReAuthenticate failed - User id is null in JWT");
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidAccessToken, description: "User id is null");
            }

            var appUser = await userManager.Users.Include(au => au.DomainUser).FirstOrDefaultAsync(au => au.DomainUserId.ToString() == domainUserId, cancellationToken: ct);
            if (appUser is null)
            {
                logger.LogWarning("ReAuthenticate failed - User not found for ID: {UserId}", domainUserId);
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found");
            }

            var currentRefreshTokenSpec = new ActiveRefreshTokenByJtiAndTokenSpec(jwt.Id, refreshToken, appUser.Id);
            var currentRefreshToken = await unitOfWork.RefreshTokens.FirstOrDefaultAsync(currentRefreshTokenSpec, ct);


            if (currentRefreshToken is null || !currentRefreshToken.IsActive)
            {
                logger.LogWarning("ReAuthenticate failed - Invalid or inactive refresh token for user: {Email}", appUser.Email);
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidRefreshToken, description: "Refresh token is not valid");
            }

            var jwtResultOperation = await AuthenticateAsync(appUser, currentRefreshToken!.Expires);
            if (jwtResultOperation.IsError)
                return jwtResultOperation.Errors;

            currentRefreshToken.Revoke();
            await unitOfWork.RefreshTokens.UpdateAsync(currentRefreshToken, ct);
            logger.LogInformation("Successful token refresh for user: {Email}", appUser.Email);
            return jwtResultOperation;
        }


        public async Task<ErrorOr<Success>> LogoutAsync(string refreshToken, CancellationToken ct = default)
        {

            var domainUserId = currentUserService.UserId!.Value;

            var appUser = await userManager.Users.FirstOrDefaultAsync
                 (au => au.DomainUserId == domainUserId, cancellationToken: ct);

            if (appUser is null)
            {
                logger.LogWarning("Logout failed - User not found for ID: {UserId}", domainUserId);
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found!");
            }

            var refreshTokenFromDb = await unitOfWork.RefreshTokens.GetAsync(r => r.Token == refreshToken && r.UserId == appUser.Id, ct);

            if (refreshTokenFromDb is null || !refreshTokenFromDb.IsActive)
            {
                logger.LogWarning("Logout failed - Invalid refresh token for user: {Email}", appUser.Email);
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidRefreshToken, description: "You maybe signed out!");
            }

            refreshTokenFromDb.Revoke();
            await unitOfWork.RefreshTokens.UpdateAsync(refreshTokenFromDb, ct);
            logger.LogInformation("Successful logout for user: {Email}", appUser.Email);
            return Result.Success;

        }


        public async Task<ErrorOr<Success>> ChangePassword(int domainUserId, string currentPassword, string newPassword)
        {
            var appUser = await userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId);
            if (appUser is null)
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");

            var result = await userManager.ChangePasswordAsync(appUser, currentPassword, newPassword);

            if (!result.Succeeded)
                return Error.Unauthorized(code: ErrorCodes.Authentication.PasswordChangeFailed, description: result.Errors.FirstOrDefault()?.Description ?? "Password change failed");

            return Result.Success;

        }


        public async Task<ErrorOr<string>> CreateEmailConfirmationCode(int domainUserId, CancellationToken ct = default)
        {
            var appUser = await userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId, cancellationToken: ct);

            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            if (appUser.EmailConfirmed)
            {
                return Error.Validation(code: ErrorCodes.Authentication.EmailAlreadyConfirmed, description: "Email is already confirmed.");
            }

            var existingCode = await unitOfWork.VerificationCodes.GetAsync(v =>
                v.ApplicationUserId == appUser.Id &&
                v.Type == VerificationCodeType.EmailConfirmation &&
                v.Status == VerificationCodeStatus.Active, ct);

            existingCode?.MarkAsRevoked();

            var emailCodeLength = _emailOptions.CodeLength;
            var confirmEmailCode = otpService.Generate(length: emailCodeLength);
            var expireInMinutes = _emailOptions.EmailExpireInMinutes;


            var codeExpiresAt = dateTimeProvider.UtcNow.AddMinutes(minutes: expireInMinutes);
            var verificationCode = new VerificationCode(appUser.Id, confirmEmailCode, VerificationCodeType.EmailConfirmation, codeExpiresAt);

            await unitOfWork.VerificationCodes.AddAsync(verificationCode, ct);

            return confirmEmailCode;
        }



        public async Task<ErrorOr<Success>> ConfirmEmail(int domainUserId, string code, CancellationToken ct = default)
        {
            var appUser = await userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId, cancellationToken: ct);

            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            if (appUser.EmailConfirmed)
            {
                return Error.Validation(code: ErrorCodes.Authentication.EmailAlreadyConfirmed, description: "Email is already confirmed.");
            }

            var verificationCode = await unitOfWork.VerificationCodes.GetAsync(v =>
                v.ApplicationUserId == appUser.Id &&
                v.Type == VerificationCodeType.EmailConfirmation &&
                v.Status == VerificationCodeStatus.Active,
                ct);

            if (verificationCode is null)
            {
                return Error.NotFound(code: ErrorCodes.Authentication.InvalidEmailConfirmationCode, description: "Invalid code");
            }


            if (verificationCode.Code != code)
            {
                var incrementAttemptsResult = verificationCode.IncrementAttempts();

                return Error.Validation(
                    code: ErrorCodes.Authentication.InvalidEmailConfirmationCode, description: "Invalid verification code.");
            }


            if (!verificationCode.CanBeUsed())
            {
                return Error.Validation(code: ErrorCodes.Authentication.InvalidEmailConfirmationCode, description: "Invalid code.");
            }


            verificationCode.MarkAsUsed();
            appUser.EmailConfirmed = true;
            return Result.Success;
        }



        #region Helper functions

        private async Task<ErrorOr<AuthResult>> AuthenticateAsync(ApplicationUser appUser, DateTimeOffset? refreshTokenExpDate = null)
        {
            if (appUser is null || appUser.DomainUserId is null || appUser.DomainUser is null)
                return Error.Validation(description: "User cannot be null");


            var userClaims = await GetUserClaims(appUser);

            var jwtSecurityToken = GenerateAccessToken(userClaims);
            var refreshToken = GenerateRefreshToken(appUser.Id, jwtSecurityToken.Id, refreshTokenExpDate);
            await unitOfWork.RefreshTokens.AddAsync(refreshToken);

            string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            RefreshTokenDTO refreshTokenDto = new()
            {
                Token = refreshToken.Token,
                UserId = refreshToken.UserId,
                ExpirationDate = refreshToken.Expires
            };

            var userResponse = mapper.Map<UserBaseResponse>(appUser.DomainUser);
            var userRole = (await userManager.GetRolesAsync(appUser)).FirstOrDefault();
            if (userRole is not null)
            {
                userResponse.UserRole = Enum.Parse<UserRole>(userRole);

            }
            userResponse.SecretKey = appUser.DomainUser.TotpSecret;

            AuthResult jwtResult = new(accessToken, refreshTokenDto, userResponse);


            return jwtResult;
        }

        private JwtSecurityToken GenerateAccessToken(List<Claim> userClaims)
        {
            return new JwtSecurityToken(
                  issuer: _jwtOptions.Issuer,
                  audience: _jwtOptions.Audience,
                  claims: userClaims,
                  signingCredentials: GetSigningCredentials(),
                  expires: DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes).UtcDateTime
              );
        }
        private async Task<List<Claim>> GetUserClaims(ApplicationUser user)
        {
            var claims = new List<Claim>()
             {
                 new(ClaimTypes.NameIdentifier,user.DomainUserId!.Value.ToString()),
                 new(ClaimTypes.Email,user.DomainUser!.Email),
                 new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                 new("UserType", user.DomainUser.UserType.ToString())


             };

            var customClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(customClaims);

            var roles = await userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));

                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null) continue;

                var roleClaims = await roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);

                var permissions = await dbContext.RolePermissions
                    .Where(rp => rp.RoleId == role.Id)
                    .Select(rp => rp.Permission)
                    .ToListAsync();

                foreach (var permission in permissions)
                {
                    claims.Add(new Claim("Permission", ((int)permission).ToString()));
                }
            }

            return claims;
        }
        private SigningCredentials GetSigningCredentials()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }
        private RefreshToken GenerateRefreshToken(int userId, string accessTokenJTI, DateTimeOffset? expirationDate = null)
        {
            var randomBytes = new byte[64];
            RandomNumberGenerator.Fill(randomBytes);
            string Token = Convert.ToBase64String(randomBytes);

            expirationDate ??= DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);
            return new RefreshToken(Token, expirationDate.Value, accessTokenJTI, userId);

        }


        private bool ValidateAccessToken(string token, bool validateLifetime = true)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.FromMinutes(2)  //default = 5 min (security gap)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return principal is not null;
        }

        private static JwtSecurityToken ReadJWT(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }

        #endregion

    }
}