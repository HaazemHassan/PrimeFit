using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Subscriptions
{
    public class BranchSubscriptionSearchSpec : Specification<Subscription>
    {

        public BranchSubscriptionSearchSpec(int branchId, string? search)
        {

            Query.Where(s => s.BranchId == branchId);

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
