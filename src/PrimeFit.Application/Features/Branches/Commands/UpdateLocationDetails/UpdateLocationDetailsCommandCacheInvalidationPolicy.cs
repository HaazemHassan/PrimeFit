using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails
{
    public class UpdateLocationDetailsCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdateLocationDetailsCommand>
    {
        public IEnumerable<string> GetTags(UpdateLocationDetailsCommand request)
        {
            yield return BranchesCache.ListTag();
            yield return BranchesCache.ByIdTag(request.BranchId);
        }
    }
}
