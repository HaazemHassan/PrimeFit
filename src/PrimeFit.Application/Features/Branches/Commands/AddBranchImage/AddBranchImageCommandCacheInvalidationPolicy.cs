using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;
using PrimeFit.Application.Features.Branches.Commands.CreateBranchImage;

namespace PrimeFit.Application.Features.Branches.Commands.AddBranchImage
{
    public class AddBranchImageCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<AddBranchImageCommand>
    {
        public IEnumerable<string> GetTags(AddBranchImageCommand request)
        {
            yield return BranchesCache.ByIdTag(request.BranchId);
        }
    }
}
