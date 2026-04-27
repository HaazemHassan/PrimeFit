using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;
using PrimeFit.Application.Features.BranchReviews.Caching;

namespace PrimeFit.Application.Features.BranchReviews.Commands.UpdateMyBranchReview
{
    public class UpdateMyBranchReviewCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdateMyBranchReviewCommand>
    {
        public IEnumerable<string> GetTags(UpdateMyBranchReviewCommand request)
        {
            yield return BranchReviewsCache.ListTag(request.BranchId);
            yield return BranchesCache.ListTag();
            yield return BranchesCache.ByIdTag(request.BranchId);
        }
    }
}
