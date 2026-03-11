using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchPackagesPaginatedSpec : Specification<Package>
    {
        public BranchPackagesPaginatedSpec(int branchId, int pageNumber, int pageSize)
        {
            Query.Where(p => p.BranchId == branchId);

            Query.OrderBy(p => p.CreatedAt).ThenBy(p => p.IsActive);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
