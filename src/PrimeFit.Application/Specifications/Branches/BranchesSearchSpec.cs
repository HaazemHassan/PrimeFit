using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.Specifications.Users
{
    public class BranchesSearchSpec : Specification<Branch>
    {
        public BranchesSearchSpec(int? ownerId, string? search)
        {

            if (ownerId.HasValue)
            {
                Query.Where(u => u.OwnerId == ownerId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(u => u.Name.Contains(search));
            }
        }
    }
}
