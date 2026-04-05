using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<ErrorOr<Success>>
    {
        public string Email { get; init; }
        public string Code { get; init; }
    }

}
