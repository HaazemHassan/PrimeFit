using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class PackageWithBrnachByIdSpec : Specification<Package>
    {
        public PackageWithBrnachByIdSpec(int packageId, int branchId)
        {
            Query.Where(p => p.Id == packageId && p.BranchId == branchId)
                 .Include(p => p.Branch);
        }
    }
}
