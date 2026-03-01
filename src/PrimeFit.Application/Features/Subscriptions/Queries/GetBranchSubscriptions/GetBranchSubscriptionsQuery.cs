using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions
{

    [Authorize(Roles = [UserRole.Owner])]
    public class GetBranchSubscriptionsQuery : PaginatedQuery, IRequest<ErrorOr<PaginatedResult<GetBranchSubscriptionsQueryResponse>>>, IAuthorizedRequest
    {
        public GetBranchSubscriptionsQuery(int branchId)
        {
            BranchId = branchId;
        }

        public int BranchId { get; set; }
        public SubscriptionStatus? SubscriptionStatus { get; set; }

    }
}
