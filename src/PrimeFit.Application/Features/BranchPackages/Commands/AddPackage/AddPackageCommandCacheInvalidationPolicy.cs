using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.BranchPackages.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Commands.AddPackage
{
    public class AddPackageCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<AddPackageCommand>
    {
        public IEnumerable<string> GetTags(AddPackageCommand request)
        {
            yield return BranchPackagesCache.Tag(request.BranchId);
        }
    }
}
