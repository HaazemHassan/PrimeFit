using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Features.Employees.Cashing;

namespace PrimeFit.Application.Features.Employees.Queries.GetEmployeeRoles
{
    public class GetEmployeeRolesQueryCashePolicy : ICachePolicy<GetEmployeeRolesQuery>
    {
        public TimeSpan? Expiration => TimeSpan.FromDays(30);

        public string GetCacheKey(GetEmployeeRolesQuery request)
        {
            return EmployeeCache.EmployeeRolesCacheKey();
        }
    }
}
