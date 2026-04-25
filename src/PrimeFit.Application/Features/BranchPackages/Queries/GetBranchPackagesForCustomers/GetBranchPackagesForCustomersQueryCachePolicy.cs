using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.BranchPackages.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForCustomers
{
    public class GetBranchPackagesForCustomersQueryCachePolicy : ICachePolicy<GetBranchPackagesForCustomersQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromHours(1);

        public string GetCacheKey(GetBranchPackagesForCustomersQuery request)
        {
            return BranchPackagesCacheKeys.Paginated(request.BranchId, request.PageNumber, request.PageSize);
        }

        public string[] GetCacheTags(GetBranchPackagesForCustomersQuery request)
        {
            return [BranchPackagesCacheKeys.Tag(request.BranchId)];
        }


        public bool ShouldSkipCache(GetBranchPackagesForCustomersQuery request)
        {
            if (request.PageNumber > 3)
            {
                return true;
            }
            return false;
        }
    }
}
