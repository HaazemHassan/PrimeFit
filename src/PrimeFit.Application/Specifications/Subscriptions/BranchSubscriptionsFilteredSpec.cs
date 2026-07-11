using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Subscriptions
{
    public class BranchSubscriptionsFilteredSpec : Specification<Subscription>
    {

        public BranchSubscriptionsFilteredSpec(int branchId, SubscriptionStatus? subscriptionStatus, string? search)
        {

            Query.Where(s => s.BranchId == branchId);

            if (subscriptionStatus.HasValue)
            {
                Query.Where(s => s.Status == subscriptionStatus.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                var searchTerms = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var term in searchTerms)
                {
                    var lowerTerm = term.ToLower();
                    Query.Where(s =>
                        s.User.FirstName.ToLower().Contains(lowerTerm) ||
                        s.User.LastName.ToLower().Contains(lowerTerm) ||
                        s.User.PhoneNumber.Contains(lowerTerm));
                }
            }


        }
    }
}
