using ErrorOr;

namespace PrimeFit.Api.Requests.Authentication.ResetPassword;

public class ResetPasswordRequest
{
        public string Email { get; init; }
        public string Code { get; init; }
        public string NewPassword { get; init; }
        public string ConfirmNewPassword { get; init; }
}
