using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic
{
    public class GetBranchesForPublicQueryCachePolicy : ICachePolicy<GetBranchesForPublicQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public string GetCacheKey(GetBranchesForPublicQuery request)
        {
            return BranchesCache.Paginated(request.PageNumber, request.PageSize);
        }

        public string[] GetCacheTags(GetBranchesForPublicQuery request)
        {
            return [BranchesCache.Tag()];
        }


        public bool ShouldSkipCache(GetBranchesForPublicQuery request)
        {
            if (request.Latitude is not null || request.Longitude is not null)
            {
                return true;
            }

            if (!string.IsNullOrEmpty(request.search))
            {
                return true;
            }

            if (request.PageNumber > 3)
            {
                return true;
            }

            return false;
        }
    }
}
