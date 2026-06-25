using PrimeFit.Application.Common.Messaging;
using PrimeFit.Domain.DomainEvents;

namespace PrimeFit.Application.Features.Subscriptions.Events
{
    public sealed class EnqueueSubscriptionCreatedIntegrationEventHandler : IDomainEventHandler<SubscriptionCreatedDomainEvent>
    {
        private readonly IOutboxIntegrationEventPublisher _publisher;

        public EnqueueSubscriptionCreatedIntegrationEventHandler(IOutboxIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public Task Handle(SubscriptionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new SubscriptionCreatedIntegrationEvent(notification.UserId, notification.BranchName);

            _publisher.Publish(integrationEvent);

            return Task.CompletedTask;
        }
    }
}
