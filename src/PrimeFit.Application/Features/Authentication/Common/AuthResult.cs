using System.Text.Json.Serialization;
using PrimeFit.Application.Features.Users.Common;

namespace PrimeFit.Application.Features.Authentication.Common
{
    public class AuthResult(string accessToken, RefreshTokenDTO refreshToken, BaseUserResponse user)
    {
        public string AccessToken { get; set; } = accessToken;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RefreshTokenDTO? RefreshToken { get; set; } = refreshToken;

        public BaseUserResponse User { get; set; } = user;
    }


    public class RefreshTokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
