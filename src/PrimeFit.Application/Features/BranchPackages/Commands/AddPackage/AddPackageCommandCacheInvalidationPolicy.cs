using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.BranchPackages.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Commands.AddPackage
{
    public class AddPackageCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<AddPackageCommand>
    {
        public IEnumerable<string> GetTags(AddPackageCommand request)
        {
            yield return BranchPackagesCache.ListTag(request.BranchId);
            yield return BranchesCache.ByIdTag(request.BranchId);
        }
    }
}
