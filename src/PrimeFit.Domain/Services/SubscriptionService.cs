using ErrorOr;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.ServicesContracts;
using PrimeFit.Domain.Specifications.Subscriptions;

namespace PrimeFit.Domain.Services
{
    public class SubscriptionService : ISubscriptionDomainService
    {

        private readonly TimeProvider _timeProvider;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(TimeProvider timeProvider, ISubscriptionRepository userRepository)
        {
            _timeProvider = timeProvider;
            _subscriptionRepository = userRepository;
        }

        public async Task<ErrorOr<Subscription>> CreateSubscriptionAsync(DomainUser user, Branch branch, Package package, CancellationToken ct)
        {

            var createSubscriptionResult = Subscription.Create(user, branch, package);

            if (createSubscriptionResult.IsError)
            {
                return createSubscriptionResult.Errors;
            }

            var subscription = createSubscriptionResult.Value;

            var UserHasActiveSubscriptionSpec = new UserActiveSubscriptionInBranchSpec(user.Id, branch.Id);
            bool isUserHasActiveSubscription = await _subscriptionRepository.AnyAsync(UserHasActiveSubscriptionSpec, ct);

            if (!isUserHasActiveSubscription)
            {
                subscription.Activate(_timeProvider.GetUtcNow());

            }


            return subscription;
        }


    }
}
