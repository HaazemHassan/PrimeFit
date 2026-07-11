using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchPackagesSpec : Specification<Package>
    {
        public BranchPackagesSpec(int branchId, string? search = null)
        {
            Query.Where(p => p.BranchId == branchId);
            
            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                Query.Where(p => p.Name.ToLower().Contains(lowerSearch));
            }
        }
    }
}
