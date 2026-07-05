using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.Governorates.Caching;

namespace PrimeFit.Application.Features.Governorates.Queries.GetGovernorates
{
    public class GetGovernoratesQueryCachingPolicy : ICachePolicy<GetGovernoratesQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromDays(30);

        public string GetCacheKey(GetGovernoratesQuery request)
        {
            return GovernoratesCache.ListCacheKey();
        }
    }
}
