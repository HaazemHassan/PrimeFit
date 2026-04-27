using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQueryCachePolicy : ICachePolicy<GetBranchByIdQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromMinutes(30);

        public string GetCacheKey(GetBranchByIdQuery request)
        {
            return BranchesCache.ById(request.BranchId);
        }

        public string[] GetCacheTags(GetBranchByIdQuery request)
        {
            return [
                BranchesCache.Tag(request.BranchId),
                BranchesCache.ListTag()
            ];
        }

        public bool ShouldSkipCache(GetBranchByIdQuery request)
        {
            return false;
        }
    }
}
