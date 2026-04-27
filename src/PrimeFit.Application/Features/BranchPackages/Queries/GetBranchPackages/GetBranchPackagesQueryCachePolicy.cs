using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackages
{
    public class GetBranchPackagesQueryCachePolicy : ICachePolicy<GetBranchPackagesQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromMinutes(30);

        public string GetCacheKey(GetBranchPackagesQuery request)
        {
            return BranchesCache.BranchPackages(request.BranchId, request.PageNumber, request.PageSize);
        }

        public string[] GetCacheTags(GetBranchPackagesQuery request)
        {
            return [
                BranchesCache.Tag(request.BranchId),
                BranchesCache.ListTag()
            ];
        }

        public bool ShouldSkipCache(GetBranchPackagesQuery request)
        {
            if (!string.IsNullOrEmpty(request.Search))
            {
                return true;
            }

            return false;
        }
    }
}
