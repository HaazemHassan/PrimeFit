using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.BranchPackages.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackageStatus
{
    public class UpdatePackageStatusCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdatePackageStatusCommand>
    {
        public IEnumerable<string> GetTags(UpdatePackageStatusCommand request)
        {
            yield return BranchPackagesCache.Tag(request.BranchId);
            yield return BranchesCache.Tag(request.BranchId);
        }
    }
}
