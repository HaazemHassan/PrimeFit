using ErrorOr;

namespace PrimeFit.Api.Requests.Authentication.Logout;

public class LogoutRequest
{
        public string? RefreshToken { get; set; }
}
