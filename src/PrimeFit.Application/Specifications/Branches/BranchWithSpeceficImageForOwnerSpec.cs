using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchWithSpeceficImageForOwnerSpec : Specification<Branch>
    {
        public BranchWithSpeceficImageForOwnerSpec(int ownerId, int branchId, int imageId)
        {
            Query.Where(b => b.Id == branchId && b.OwnerId == ownerId)
           .Include(b => b.Images.Where(i => i.Id == imageId));
        }
    }
}
