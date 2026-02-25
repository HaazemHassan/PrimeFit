using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Packages
{
    public class PackagesPaginatedForOwnerSpec : Specification<Package>
    {
        public PackagesPaginatedForOwnerSpec(int branchId, int ownerId, int pageNumber, int pageSize)
        {
            Query.Where(p => p.BranchId == branchId && p.Branch.OwnerId == ownerId);


            Query.OrderBy(p => p.CreatedAt).ThenBy(p => p.IsActive);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
