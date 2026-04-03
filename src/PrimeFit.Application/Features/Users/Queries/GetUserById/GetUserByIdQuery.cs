using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Users.Queries.GetUserById
{
    [Authorize(Policy = AuthorizationPolicies.SelfOnly)]
    public class GetUserByIdQuery : IRequest<ErrorOr<GetUserByIdQueryResponse>>, IOwnedResourceRequest
    {
        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
