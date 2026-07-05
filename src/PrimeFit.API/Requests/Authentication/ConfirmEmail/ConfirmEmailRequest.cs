using ErrorOr;

namespace PrimeFit.Api.Requests.Authentication.ConfirmEmail;

public class ConfirmEmailRequest
{
        public string Email { get; init; }
        public string Code { get; init; }
}
