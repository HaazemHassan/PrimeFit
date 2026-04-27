using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;
using PrimeFit.Application.Features.BranchReviews.Caching;

namespace PrimeFit.Application.Features.BranchReviews.Commands.AddBranchReview
{
    public class AddBranchReviewCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<AddBranchReviewCommand>
    {
        public IEnumerable<string> GetTags(AddBranchReviewCommand request)
        {
            yield return BranchReviewsCache.ListTag(request.BranchId);
            yield return BranchesCache.ListTag();
        }
    }
}
