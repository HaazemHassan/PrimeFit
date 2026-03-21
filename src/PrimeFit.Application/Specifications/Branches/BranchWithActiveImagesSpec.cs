using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Branches
{
    public class BranchWithActiveImagesSpec : Specification<Branch>
    {
        public BranchWithActiveImagesSpec(int branchId)
        {
            Query.Where(b => b.Id == branchId)
                 .Include(b => b.Images.Where(i => i.Status == BranchImageStatus.Active));

        }
    }
}
