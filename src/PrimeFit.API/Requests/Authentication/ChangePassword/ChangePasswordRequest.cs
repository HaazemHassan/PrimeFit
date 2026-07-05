using ErrorOr;

namespace PrimeFit.Api.Requests.Authentication.ChangePassword;

public class ChangePasswordRequest
{
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
}
