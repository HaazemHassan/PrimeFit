using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Authentication.Commands.ResendConfirmEmail
{
    public class ResendConfirmationEmailCommand() : IRequest<ErrorOr<Success>>
    {
        public string Email { get; set; } = null!;
    }

}
