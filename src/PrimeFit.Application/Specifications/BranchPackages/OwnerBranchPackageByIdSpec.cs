using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class OwnerBranchPackageByIdSpec : Specification<Package>
    {
        public OwnerBranchPackageByIdSpec(int packageId, int branchId, int ownerId)
        {
            Query.Where(p => p.Id == packageId && p.BranchId == branchId && p.Branch.OwnerId == ownerId)
                .Include(p => p.Branch);
        }
    }
}
