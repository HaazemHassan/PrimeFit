using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Employees.Commands.CreateEmployee;
using PrimeFit.Application.Security.Caching;

namespace PrimeFit.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<CreateEmployeeCommand>
    {
        public IEnumerable<string> GetTags(CreateEmployeeCommand request)
        {
            yield return SecurityCache.BranchAuthTag(request.BranchId);
        }
    }
}
