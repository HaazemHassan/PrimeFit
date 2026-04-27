using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Features.Branches.Caching;

namespace PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription
{
    public class AddSubscriptionCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<AddSubscriptionCommand>
    {
        public IEnumerable<string> GetTags(AddSubscriptionCommand request)
        {
            yield return BranchesCache.ByIdTag(request.BranchId);
        }
    }
}
