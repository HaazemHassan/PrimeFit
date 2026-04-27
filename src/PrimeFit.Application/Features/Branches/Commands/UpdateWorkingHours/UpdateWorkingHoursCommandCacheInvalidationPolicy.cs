using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateWorkingHours
{
    public class UpdateWorkingHoursCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdateWorkingHoursCommand>
    {
        public IEnumerable<string> GetTags(UpdateWorkingHoursCommand request)
        {
            yield return BranchesCache.ByIdTag(request.BranchId);
            yield return BranchesCache.ListTag();
        }
    }
}
