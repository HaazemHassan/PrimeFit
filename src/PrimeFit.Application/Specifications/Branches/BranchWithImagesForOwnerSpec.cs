using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchWithImagesForOwnerSpec : Specification<Branch>
    {
        public BranchWithImagesForOwnerSpec(int ownerId, int branchId)
        {
            Query.Where(b => b.Id == branchId && b.OwnerId == ownerId);
            Query.Include(b => b.Images);

        }
    }
}