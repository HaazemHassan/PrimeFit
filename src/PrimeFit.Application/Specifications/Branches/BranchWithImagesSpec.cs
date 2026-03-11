using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchWithImagesSpec : Specification<Branch>
    {
        public BranchWithImagesSpec(int branchId)
        {
            Query.Where(b => b.Id == branchId);
            Query.Include(b => b.Images);

        }
    }
}