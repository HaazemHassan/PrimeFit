using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Employees.Commands.UpdateEmployee;
using PrimeFit.Application.Security.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdateEmployeeCommand>
    {
        public IEnumerable<string> GetTags(UpdateEmployeeCommand request)
        {
            yield return SecurityCache.BranchAuthTag(request.BranchId);
            yield return BranchesCache.Tag(request.BranchId);
        }
    }
}
