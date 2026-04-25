using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.BranchPackages.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackage
{
    public class UpdatePackageCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdatePackageCommand>
    {
        public IEnumerable<string> GetTags(UpdatePackageCommand request)
        {
            yield return BranchPackagesCacheKeys.Tag(request.BranchId);
        }
    }
}
