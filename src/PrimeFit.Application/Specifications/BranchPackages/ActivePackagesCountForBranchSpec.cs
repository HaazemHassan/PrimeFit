using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Packages
{
    public class ActivePackagesCountForBranchSpec : Specification<Package>
    {
        public ActivePackagesCountForBranchSpec(int branchId)
        {
            Query.Where(p => p.BranchId == branchId && p.IsActive);

        }
    }
}
