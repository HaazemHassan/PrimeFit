using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Users.Queries.GetMe
{
    [Authorize]
    public class GetMeQuery : IRequest<ErrorOr<GetMeQueryResponse>>, IAuthorizedRequest
    {
    }
}
