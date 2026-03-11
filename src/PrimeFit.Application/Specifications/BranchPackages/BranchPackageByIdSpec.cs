using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchPackageByIdSpec : Specification<Package>
    {
        public BranchPackageByIdSpec(int packageId, int branchId)
        {
            Query.Where(p => p.Id == packageId && p.BranchId == branchId);
        }
    }
}
