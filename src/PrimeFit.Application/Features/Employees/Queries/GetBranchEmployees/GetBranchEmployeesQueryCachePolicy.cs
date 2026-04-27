using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Employees.Queries.GetBranchEmployees
{
    public class GetBranchEmployeesQueryCachePolicy : ICachePolicy<GetBranchEmployeesQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromMinutes(15);

        public string GetCacheKey(GetBranchEmployeesQuery request)
        {
            return BranchesCache.BranchEmployees(request.BranchId, request.PageNumber, request.PageSize);
        }

        public string[] GetCacheTags(GetBranchEmployeesQuery request)
        {
            return [
                BranchesCache.Tag(request.BranchId),
                BranchesCache.ListTag()
            ];
        }

        public bool ShouldSkipCache(GetBranchEmployeesQuery request)
        {
            if (!string.IsNullOrEmpty(request.Search))
            {
                return true;
            }

            return false;
        }
    }
}
