using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Branches.Queries.GetMyBranches
{
    public class GetMyBranchesQueryCachePolicy : ICachePolicy<GetMyBranchesQuery>
    {
        private readonly ICurrentUserService _currentUserService;

        public GetMyBranchesQueryCachePolicy(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public TimeSpan? Expiration => TimeSpan.FromMinutes(15);

        public string GetCacheKey(GetMyBranchesQuery request)
        {
            return BranchesCache.OwnerPaginatedCacheKey(_currentUserService.UserId!.Value, request.PageNumber, request.PageSize);
        }

        public string[] GetCacheTags(GetMyBranchesQuery request)
        {
            return [
                BranchesCache.OwnerTag(_currentUserService.UserId!.Value),
                BranchesCache.ListTag()
            ];
        }

        public bool ShouldSkipCache(GetMyBranchesQuery request)
        {
            if (!string.IsNullOrEmpty(request.Search))
            {
                return true;
            }

            return false;
        }
    }
}
