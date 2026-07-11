using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchActivePackagesSpec : Specification<Package>
    {
        public BranchActivePackagesSpec(int branchId, string? search = null)
        {
            Query.Where(p => p.BranchId == branchId && p.IsActive);
            
            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                Query.Where(p => p.Name.ToLower().Contains(lowerSearch));
            }

        }
    }
}
