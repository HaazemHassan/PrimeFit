using PrimeFit.Application.Features.Users.Common;
using System.Text.Json.Serialization;

namespace PrimeFit.Application.Features.Authentication.Common
{
    public class AuthResult(string accessToken, RefreshTokenDTO refreshToken, UserBaseResponse user)
    {
        public string AccessToken { get; set; } = accessToken;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RefreshTokenDTO? RefreshToken { get; set; } = refreshToken;

        public UserBaseResponse User { get; set; } = user;
    }


    public class RefreshTokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}
