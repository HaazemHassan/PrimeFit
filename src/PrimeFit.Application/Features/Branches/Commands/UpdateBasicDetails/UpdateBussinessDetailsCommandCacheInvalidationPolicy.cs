using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails
{
    public class UpdateBussinessDetailsCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdateBussinessDetailsCommand>
    {
        public IEnumerable<string> GetTags(UpdateBussinessDetailsCommand request)
        {
            yield return BranchesCache.Tag();
        }
    }
}
