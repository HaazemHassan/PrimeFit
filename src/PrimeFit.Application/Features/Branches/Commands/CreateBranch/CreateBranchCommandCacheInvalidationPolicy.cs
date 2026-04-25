using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Commands.CreateBranch
{
    public class CreateBranchCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<CreateBranchCommand>
    {
        public IEnumerable<string> GetTags(CreateBranchCommand request)
        {
            yield return BranchesCache.Tag();
        }
    }
}
