using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchByIdForPublic
{
    public class GetBranchByIdForPublicQueryCachePolicy : ICachePolicy<GetBranchByIdForPublicQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromSeconds(30);     //short time because of IsOpen property

        public string GetCacheKey(GetBranchByIdForPublicQuery request)
        {
            return BranchesCache.ById(request.BranchId);
        }

        public string[] GetCacheTags(GetBranchByIdForPublicQuery request)
        {
            return [
                    BranchesCache.Tag(request.BranchId),
                    BranchesCache.ListTag()
                   ];
        }


        public bool ShouldSkipCache(GetBranchByIdForPublicQuery request)
        {
            if (request.Latitude is not null || request.Longitude is not null)
            {
                return true;
            }

            return false;
        }
    }
}
