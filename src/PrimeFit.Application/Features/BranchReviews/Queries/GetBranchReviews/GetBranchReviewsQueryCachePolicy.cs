using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.BranchReviews.Caching;

namespace PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews
{
    public class GetBranchReviewsQueryCachePolicy : ICachePolicy<GetBranchReviewsQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromMinutes(30);

        public string GetCacheKey(GetBranchReviewsQuery request)
        {
            return BranchReviewsCache.Paginated(request.BranchId, request.PageNumber, request.PageSize);
        }

        public string[] GetCacheTags(GetBranchReviewsQuery request)
        {
            return [BranchReviewsCache.Tag(request.BranchId)];
        }


        public bool ShouldSkipCache(GetBranchReviewsQuery request)
        {
            if (request.PageNumber > 3)
            {
                return true;
            }

            return false;
        }
    }
}
