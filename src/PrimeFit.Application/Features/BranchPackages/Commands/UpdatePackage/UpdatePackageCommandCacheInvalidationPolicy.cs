using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.BranchPackages.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackage
{
    public class UpdatePackageCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdatePackageCommand>
    {
        public IEnumerable<string> GetTags(UpdatePackageCommand request)
        {
            yield return BranchPackagesCache.ListTag(request.BranchId);
            yield return BranchesCache.ByIdTag(request.BranchId);
        }
    }
}
