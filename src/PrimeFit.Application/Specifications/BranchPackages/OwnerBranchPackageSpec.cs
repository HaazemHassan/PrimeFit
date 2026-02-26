using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class OwnerBranchPackageSpec : Specification<Package>
    {
        public OwnerBranchPackageSpec(int branchId, int ownerId)
        {
            Query.Where(p => p.BranchId == branchId && p.Branch.OwnerId == ownerId);

        }
    }
}
