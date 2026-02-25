using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Packages
{
    public class ActivePackagesPaginatedForBranchSpec : Specification<Package>
    {
        public ActivePackagesPaginatedForBranchSpec(int branchId, int pageNumber, int pageSize)
        {
            Query.Where(p => p.BranchId == branchId && p.IsActive);


            Query.OrderBy(p => p.CreatedAt).ThenBy(p => p.IsActive);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
