using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class OwnerBranchPackagesSpec : Specification<Package>
    {
        public OwnerBranchPackagesSpec(int branchId, int ownerId)
        {
            Query.Where(p => p.BranchId == branchId && p.Branch.OwnerId == ownerId);
            Query.AsNoTracking();
        }
    }
}
