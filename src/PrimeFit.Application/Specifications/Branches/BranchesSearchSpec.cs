using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
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
                var lowerSearch = search.ToLower();
                Query.Where(u => u.Name.ToLower().Contains(lowerSearch));
            }
        }
    }
}
