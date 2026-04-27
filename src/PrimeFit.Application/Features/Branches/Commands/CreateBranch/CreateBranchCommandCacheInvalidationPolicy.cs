using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;

using PrimeFit.Application.Contracts.Api;

namespace PrimeFit.Application.Features.Branches.Commands.CreateBranch
{
    public class CreateBranchCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<CreateBranchCommand>
    {
        private readonly ICurrentUserService _currentUserService;

        public CreateBranchCommandCacheInvalidationPolicy(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public IEnumerable<string> GetTags(CreateBranchCommand request)
        {
            yield return BranchesCache.OwnerTag(_currentUserService.UserId!.Value);
            yield return BranchesCache.ListTag();
        }
    }
}
