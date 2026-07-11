using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchesFilteredSpec : Specification<Branch>
    {
        public BranchesFilteredSpec(int? ownerId, string? search)
        {
            if (ownerId.HasValue)
            {
                Query.Where(b => b.OwnerId == ownerId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                Query.Where(b => b.Name.ToLower().Contains(lowerSearch));
            }
        }
    }
}
