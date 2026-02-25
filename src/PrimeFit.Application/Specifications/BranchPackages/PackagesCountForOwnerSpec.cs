using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Packages
{
    public class PackagesCountForOwnerSpec : Specification<Package>
    {
        public PackagesCountForOwnerSpec(int branchId, int ownerId)
        {
            Query.Where(p => p.BranchId == branchId && p.Branch.OwnerId == ownerId);

        }
    }
}
