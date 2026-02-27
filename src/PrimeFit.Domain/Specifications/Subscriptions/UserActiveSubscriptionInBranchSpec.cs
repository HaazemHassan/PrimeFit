using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.Specifications.Subscriptions
{
    public class UserActiveSubscriptionInBranchSpec : Specification<Subscription>
    {
        public UserActiveSubscriptionInBranchSpec(int userId, int branchId)
        {
            Query.Where(sub => sub.UserId == userId && sub.BranchId == branchId && sub.Status == SubscriptionStatus.Active);
        }
    }
}
