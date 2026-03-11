using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionById
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class GetSubscriptionByIdQuery : IRequest<ErrorOr<GetSubscriptionByIdQueryResponse>>, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }

    }
}
