using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchActivePackagesSpec : Specification<Package>
    {
        public BranchActivePackagesSpec(int branchId)
        {
            Query.Where(p => p.BranchId == branchId && p.IsActive);

        }
    }
}
