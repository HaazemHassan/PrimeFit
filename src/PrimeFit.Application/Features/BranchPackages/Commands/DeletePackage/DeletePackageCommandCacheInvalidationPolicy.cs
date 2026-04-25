using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.BranchPackages.Caching;

namespace PrimeFit.Application.Features.BranchPackages.Commands.DeletePackage
{
    public class DeletePackageCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<DeletePackageCommand>
    {
        public IEnumerable<string> GetTags(DeletePackageCommand request)
        {
            yield return BranchPackagesCacheKeys.Tag(request.BranchId);
        }
    }
}
