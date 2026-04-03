using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.Entities;
using PrimeFit.Infrastructure.Common.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PrimeFit.Infrastructure.Services
{
    internal class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public string GenerateAccessToken(List<Claim> userClaims)
        {
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: userClaims,
                signingCredentials: GetSigningCredentials(),
                expires: DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes).UtcDateTime
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(int userId, string accessTokenJti, DateTimeOffset? expirationDate = null)
        {
            var randomBytes = new byte[64];
            RandomNumberGenerator.Fill(randomBytes);
            string token = Convert.ToBase64String(randomBytes);

            expirationDate ??= DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);
            return new RefreshToken(token, expirationDate.Value, accessTokenJti, userId);
        }

        public bool ValidateAccessToken(string token, bool validateLifetime = true)
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
                ClockSkew = TimeSpan.FromMinutes(2)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return principal is not null;
        }

        public string? GetClaimValue(string accessToken, string claimType)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

        public string? GetTokenId(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            return token.Id;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
