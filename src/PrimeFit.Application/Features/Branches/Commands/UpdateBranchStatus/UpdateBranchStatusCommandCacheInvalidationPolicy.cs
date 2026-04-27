using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;
using PrimeFit.Application.Features.BranchReviews.Caching;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBranchStatus
{
    public class UpdateBranchStatusCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdateBranchStatusCommand>
    {
        public IEnumerable<string> GetTags(UpdateBranchStatusCommand request)
        {
            yield return BranchesCache.ListTag();
            yield return BranchesCache.Tag(request.BranchId);
            yield return BranchReviewsCache.Tag(request.BranchId);

        }
    }
}
