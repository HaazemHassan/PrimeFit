using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.Specifications.Subscriptions
{
    public class UserBranchRunningSubscriptionSpec : Specification<Subscription>
    {
        public UserBranchRunningSubscriptionSpec(int userId, int branchId)
        {
            Query.Where(sub =>
             sub.UserId == userId &&
             sub.BranchId == branchId &&
             (
                 sub.Status == SubscriptionStatus.Scheduled ||
                 sub.Status == SubscriptionStatus.Active ||
                 sub.Status == SubscriptionStatus.Frozen
             ));
        }
    }
}
