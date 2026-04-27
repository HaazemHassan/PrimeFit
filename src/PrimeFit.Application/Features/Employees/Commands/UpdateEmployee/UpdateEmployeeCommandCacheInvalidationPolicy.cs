using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Employees.Commands.UpdateEmployee;
using PrimeFit.Application.Security.Caching;

namespace PrimeFit.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdateEmployeeCommand>
    {
        public IEnumerable<string> GetTags(UpdateEmployeeCommand request)
        {
            yield return SecurityCache.BranchAuthTag(request.BranchId);
        }
    }
}
