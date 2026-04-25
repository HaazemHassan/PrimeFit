using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.BranchPackages.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackageStatus
{
    public class UpdatePackageStatusCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdatePackageStatusCommand>
    {
        public IEnumerable<string> GetTags(UpdatePackageStatusCommand request)
        {
            yield return BranchPackagesCacheKeys.Tag(request.BranchId);
        }
    }
}
