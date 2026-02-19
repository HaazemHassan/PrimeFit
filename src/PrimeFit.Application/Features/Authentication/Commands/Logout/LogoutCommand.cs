using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;

namespace PrimeFit.Application.Features.Authentication.Commands.Logout
{
    [Authorize]
    public class LogoutCommand : IRequest<ErrorOr<Success>>
    {
        public string? RefreshToken { get; set; }
    }
}
