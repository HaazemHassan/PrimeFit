using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class BranchPackageWithBranchSpec : Specification<Package>
    {
        public BranchPackageWithBranchSpec(int packageId)
        {

            Query.Where(p => p.Id == packageId);

            Query.Include(p => p.Branch);
        }
    }
}
