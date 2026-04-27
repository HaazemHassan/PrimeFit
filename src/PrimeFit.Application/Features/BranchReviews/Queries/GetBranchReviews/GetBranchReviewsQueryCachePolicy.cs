using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.BranchReviews.Caching;

namespace PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews
{
    public class GetBranchReviewsQueryCachePolicy : ICachePolicy<GetBranchReviewsQuery>
    {

        private readonly ICurrentUserService _currentUserService;

        public GetBranchReviewsQueryCachePolicy(
            ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        public TimeSpan? Expiration => TimeSpan.FromMinutes(30);

        public string GetCacheKey(GetBranchReviewsQuery request)
        {
            return BranchReviewsCache.PaginatedCacheKey(request.BranchId, request.PageNumber, request.PageSize);
        }

        public string[] GetCacheTags(GetBranchReviewsQuery request)
        {
            return [BranchReviewsCache.ListTag(request.BranchId)];
        }


        public bool ShouldSkipCache(GetBranchReviewsQuery request)
        {

            if (_currentUserService.IsAuthenticated)  //because of myReview Field in the response
            {
                return true;
            }


            if (request.PageNumber > 3)
            {
                return true;
            }

            if (request.Rating.HasValue)
            {
                return true;
            }

            return false;
        }
    }
}
