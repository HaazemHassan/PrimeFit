using System.Security.Claims;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Contracts.Infrastructure
{
    public interface ITokenService
    {
        string GenerateAccessToken(List<Claim> userClaims);
        RefreshToken GenerateRefreshToken(int userId, string accessTokenJti, DateTimeOffset? expirationDate = null);
        bool ValidateAccessToken(string token, bool validateLifetime = true);
        string? GetClaimValue(string accessToken, string claimType);
        string? GetTokenId(string accessToken);
    }
}
