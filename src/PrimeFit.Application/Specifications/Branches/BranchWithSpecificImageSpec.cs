using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchWithSpecificImageSpec : Specification<Branch>
    {
        public BranchWithSpecificImageSpec(int branchId, int imageId)
        {
            Query.Where(b => b.Id == branchId)
           .Include(b => b.Images.Where(i => i.Id == imageId));
        }
    }
}
