using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.SubscriptionsView])]
    public class GetBranchSubscriptionsQuery : PaginatedQuery, IRequest<ErrorOr<PaginatedResult<GetBranchSubscriptionsQueryResponse>>>, IBranchAuthorizedRequest
    {
        public int BranchId { get; set; }
        public SubscriptionStatus? SubscriptionStatus { get; set; }

    }
}
