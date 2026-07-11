using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchPackagesFilteredSpec : Specification<Package>
    {
        public BranchPackagesFilteredSpec(int branchId, string? search = null, bool? isActive = null)
        {
            Query.Where(p => p.BranchId == branchId);

            if (isActive.HasValue)
            {
                Query.Where(p => p.IsActive == isActive.Value);
            }
            
            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                Query.Where(p => p.Name.ToLower().Contains(lowerSearch));
            }
        }
    }
}
