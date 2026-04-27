using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.Branches.Caching;
using PrimeFit.Application.Features.BranchPackages.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackages
{
    public class GetBranchPackagesQueryCachePolicy : ICachePolicy<GetBranchPackagesQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromMinutes(30);

        public string GetCacheKey(GetBranchPackagesQuery request)
        {
            return BranchPackagesCache.AdminPaginatedCacheKey(request.BranchId, request.PageNumber, request.PageSize);
        }

        public string[] GetCacheTags(GetBranchPackagesQuery request)
        {
            return [
                BranchesCache.ByIdTag(request.BranchId),
                BranchPackagesCache.ListTag(request.BranchId)
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
