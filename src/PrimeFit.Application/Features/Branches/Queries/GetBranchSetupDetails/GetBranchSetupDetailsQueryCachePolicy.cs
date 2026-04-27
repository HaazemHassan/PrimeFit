using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails
{
    public class GetBranchSetupDetailsQueryCachePolicy : ICachePolicy<GetBranchSetupDetailsQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromMinutes(30);

        public string GetCacheKey(GetBranchSetupDetailsQuery request)
        {
            return BranchesCache.SetupDetailsCacheKey(request.BranchId);
        }

        public string[] GetCacheTags(GetBranchSetupDetailsQuery request)
        {
            return [
                BranchesCache.ByIdTag(request.BranchId),
                BranchesCache.ListTag()
            ];
        }

        public bool ShouldSkipCache(GetBranchSetupDetailsQuery request)
        {
            return false;
        }
    }
}
