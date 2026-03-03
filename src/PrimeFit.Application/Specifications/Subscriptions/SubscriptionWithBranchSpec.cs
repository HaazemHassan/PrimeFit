using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Subscriptions
{
    public class SubscriptionWithBranchSpec : Specification<Subscription>
    {
        public SubscriptionWithBranchSpec(int subscriptionId)
        {
            Query.Where(s => s.Id == subscriptionId)
                 .Include(s => s.Branch);

        }
    }
}
