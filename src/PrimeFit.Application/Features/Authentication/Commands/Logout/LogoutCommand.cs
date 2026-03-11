using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Authentication.Commands.Logout
{
    [Authorize]
    public class LogoutCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest
    {
        public string? RefreshToken { get; set; }
    }
}
