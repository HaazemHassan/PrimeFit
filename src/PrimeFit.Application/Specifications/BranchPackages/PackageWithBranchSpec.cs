using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchPackages
{
    public class PackageWithBranchSpec : Specification<Package>
    {
        public PackageWithBranchSpec(int packageId)
        {

            Query.Where(p => p.Id == packageId);

            Query.Include(p => p.Branch);
        }
    }
}
