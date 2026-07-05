using ErrorOr;

namespace PrimeFit.Api.Requests.Authentication.RefreshToken;

public class RefreshTokenRequest
{
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
}
