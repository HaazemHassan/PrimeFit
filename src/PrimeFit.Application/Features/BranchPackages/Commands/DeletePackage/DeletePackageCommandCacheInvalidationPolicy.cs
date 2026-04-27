using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.BranchPackages.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Commands.DeletePackage
{
    public class DeletePackageCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<DeletePackageCommand>
    {
        public IEnumerable<string> GetTags(DeletePackageCommand request)
        {
            yield return BranchPackagesCache.ListTag(request.BranchId);
            yield return BranchesCache.ByIdTag(request.BranchId);
        }
    }
}
