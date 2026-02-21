using PrimeFit.Application.Features.Users.Common;

namespace PrimeFit.Application.Features.Users.Commands.UpdateProfile {
    public class UpdateProfileCommandResponse : BaseUserResponse {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
