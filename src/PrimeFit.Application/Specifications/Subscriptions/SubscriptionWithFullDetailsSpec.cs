using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Subscriptions
{
    public class SubscriptionWithFullDetailsSpec : Specification<Subscription>
    {

        public SubscriptionWithFullDetailsSpec(int subscriptionId, int? ownerId, int? subscriperId)
        {

            Query.Where(s => s.Id == subscriptionId);


            if (ownerId.HasValue)
            {
                Query.Where(s => s.Branch.OwnerId == ownerId.Value);

            }
            if (subscriperId.HasValue)
            {
                Query.Where(s => s.UserId == subscriperId.Value);

            }

            Query.Include(s => s.Package)
                 .Include(s => s.User)
                 .Include(s => s.Freezes);

        }
    }
}
