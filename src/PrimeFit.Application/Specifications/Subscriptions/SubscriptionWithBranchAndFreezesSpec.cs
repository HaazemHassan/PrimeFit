using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Subscriptions
{
    public class SubscriptionWithBranchAndFreezesSpec : Specification<Subscription>
    {

        public SubscriptionWithBranchAndFreezesSpec(int subscriptionId)
        {
            Query.Where(s => s.Id == subscriptionId)
                 .Include(s => s.Branch).Include(s => s.Freezes);

        }

    }
}
