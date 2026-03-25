using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ErrorOr<Success>>
    {
        public string Email { get; init; }
        public string Code { get; init; }
        public string NewPassword { get; init; }
        public string ConfirmNewPassword { get; init; }
    }
}
