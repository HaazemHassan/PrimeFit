using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Commands.ActivateBranchImages
{
    public class ActivateBranchImagesCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<ActivateBranchImagesCommand>
    {
        public IEnumerable<string> GetTags(ActivateBranchImagesCommand request)
        {
            yield return BranchesCache.Tag();
        }
    }
}
