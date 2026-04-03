using AutoMapper;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.Features.Authentication.Commands.SignIn;
using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Domain.Specifications.RefreshTokens;
using PrimeFit.Infrastructure.Data;
using PrimeFit.Infrastructure.Data.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PrimeFit.Infrastructure.Services
{
    internal class AuthenticationService(
          UserManager<ApplicationUser> userManager,
          IUnitOfWork unitOfWork,
          IMapper mapper,
          RoleManager<ApplicationRole> roleManager,
          ICurrentUserService currentUserService,
          AppDbContext dbContext,
          ILogger<AuthenticationService> logger,
          ITokenService tokenService) : IAuthenticationService
    {



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
            var isValidAccessToken = tokenService.ValidateAccessToken(accessToken, validateLifetime: false);
            if (!isValidAccessToken)
            {
                logger.LogWarning("ReAuthenticate failed - Invalid access token");
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidAccessToken, description: "Invalid access token");
            }

            var tokenId = tokenService.GetTokenId(accessToken);
            if (string.IsNullOrWhiteSpace(tokenId))
            {
                logger.LogWarning("ReAuthenticate failed - Cannot read JWT");
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidAccessToken, description: "Invalid access token");
            }

            var domainUserId = tokenService.GetClaimValue(accessToken, ClaimTypes.NameIdentifier);
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

            var currentRefreshTokenSpec = new ActiveRefreshTokenByJtiAndTokenSpec(tokenId, refreshToken, appUser.Id);
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



        #region Helper functions

        private async Task<ErrorOr<AuthResult>> AuthenticateAsync(ApplicationUser appUser, DateTimeOffset? refreshTokenExpDate = null)
        {
            if (appUser is null || appUser.DomainUserId is null || appUser.DomainUser is null)
                return Error.Validation(description: "User cannot be null");


            var userClaims = await GetUserClaims(appUser);

            var accessToken = tokenService.GenerateAccessToken(userClaims);
            var jti = userClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrWhiteSpace(jti))
            {
                return Error.Failure(code: ErrorCodes.Authentication.InvalidAccessToken, description: "Invalid token payload.");
            }

            var refreshToken = tokenService.GenerateRefreshToken(appUser.Id, jti, refreshTokenExpDate);
            await unitOfWork.RefreshTokens.AddAsync(refreshToken);

            RefreshTokenDTO refreshTokenDto = new()
            {
                Token = refreshToken.Token,
                UserId = refreshToken.UserId,
                ExpirationDate = refreshToken.Expires
            };

            var userResponse = mapper.Map<UserBaseResponse>(appUser.DomainUser);

            userResponse.EmailConfirmed = appUser.EmailConfirmed;

            var userRole = (await userManager.GetRolesAsync(appUser)).FirstOrDefault();
            if (userRole is not null)
            {
                userResponse.UserRole = Enum.Parse<UserRole>(userRole);

            }
            userResponse.SecretKey = appUser.DomainUser.TotpSecret;

            if (appUser.DomainUser.UserType == UserType.Employee)
            {
                var employeeContext = await dbContext.Employees
                  .Where(e => e.UserId == appUser.DomainUserId)
                  .Select(e => new EmployeeBranchContextDto
                  {
                      BranchId = e.BranchId,
                      BranchName = e.Branch.Name,
                      RoleId = e.RoleId,
                      RoleName = e.Role.Name
                  })
                  .FirstOrDefaultAsync();

                userResponse.WorksAtBranch = employeeContext;

            }

            AuthResult jwtResult = new(accessToken, refreshTokenDto, userResponse);

            return jwtResult;
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

        #endregion

    }
}