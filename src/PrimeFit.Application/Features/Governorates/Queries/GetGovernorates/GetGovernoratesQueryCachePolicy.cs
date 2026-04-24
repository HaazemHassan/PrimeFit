using PrimeFit.Application.Common.Cashing;

namespace PrimeFit.Application.Features.Governorates.Queries.GetGovernorates
{
    public class GetGovernoratesCachePolicy : ICachePolicy<GetGovernoratesQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromHours(24);

        public string GetCacheKey(GetGovernoratesQuery request)
            => "lookups:governorates";
    }
}
