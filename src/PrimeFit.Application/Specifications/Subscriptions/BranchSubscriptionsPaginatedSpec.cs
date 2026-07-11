using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Subscriptions
{
    public class BranchSubscriptionsPaginatedSpec : BranchSubscriptionsFilteredSpec
    {
        public BranchSubscriptionsPaginatedSpec(int branchId,
            SubscriptionStatus? subscriptionStatus,
            string? search,
            int pageNumber, int pageSize)
            : base(branchId, subscriptionStatus, search)
        {

            Query.Include(s => s.User)
                 .Include(s => s.Freezes);

            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
