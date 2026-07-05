using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails
{
    public class UpdateBasicDetailsCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdateBasicDetailsCommand>
    {
        public IEnumerable<string> GetTags(UpdateBasicDetailsCommand request)
        {
            yield return BranchesCache.ListTag();
            yield return BranchesCache.ByIdTag(request.BranchId);
        }
    }
}
