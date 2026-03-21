using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Commands.ActivateBranchImages
{
    public class BranchWithActiveAndSelectedPendingImagesSpec : Specification<Branch>
    {
        public BranchWithActiveAndSelectedPendingImagesSpec(int branchId, List<int> pendingImageIds)
        {
            Query.Where(b => b.Id == branchId);

            if (pendingImageIds.Count > 0)
            {
                Query.Include(b => b.Images
                     .Where(i =>
                        i.Status == BranchImageStatus.Active ||
                        (i.Status == BranchImageStatus.Pending &&
                         pendingImageIds.Contains(i.Id))));
            }
            else
            {
                Query.Include(b => b.Images
                     .Where(i => i.Status == BranchImageStatus.Active));
            }

        }
    }
}