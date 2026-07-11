using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchPackagesPaginatedSpec : Specification<Package>
    {
        public BranchPackagesPaginatedSpec(int branchId, int pageNumber, int pageSize, string? search = null)
        {
            Query.Where(p => p.BranchId == branchId);
            
            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                Query.Where(p => p.Name.ToLower().Contains(lowerSearch));
            }

            Query.OrderBy(p => p.CreatedAt).ThenBy(p => p.IsActive);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
