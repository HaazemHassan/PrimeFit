using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Authentication.Commands.ConfirmEmail
{
    [Authorize]
    public record ConfirmEmailCommand(string code) : IRequest<ErrorOr<Success>>, IAuthorizedRequest;

}
