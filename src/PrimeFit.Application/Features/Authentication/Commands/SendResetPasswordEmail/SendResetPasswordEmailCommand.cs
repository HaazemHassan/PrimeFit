using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Authentication.Commands.SendResetPasswordEmail
{
    public class SendResetPasswordEmailCommand() : IRequest<ErrorOr<Success>>
    {
        public string Email { get; set; } = null!;
    }
}
