using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;
using PrimeFit.Application.Features.Branches.Commands.CreateMemberWithSubscription;

namespace PrimeFit.Application.Features.Subscriptions.Commands.CreateMemberWithSubscription
{
    public class CreateMemberWithSubscriptionCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<CreateMemberWithSubscriptionCommand>
    {
        public IEnumerable<string> GetTags(CreateMemberWithSubscriptionCommand request)
        {
            yield return BranchesCache.ByIdTag(request.BranchId);
        }
    }
}
