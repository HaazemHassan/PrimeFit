using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchPackagesSpec : Specification<Package>
    {
        public BranchPackagesSpec(int branchId)
        {
            Query.Where(p => p.BranchId == branchId);
        }
    }
}
