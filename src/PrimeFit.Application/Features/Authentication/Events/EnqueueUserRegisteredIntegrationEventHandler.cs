using PrimeFit.Application.Common.Messaging;
using PrimeFit.Domain.DomainEvents;

namespace PrimeFit.Application.Features.Authentication.Events
{
    public sealed class EnqueueUserRegisteredIntegrationEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        private readonly IOutboxIntegrationEventPublisher _publisher;

        public EnqueueUserRegisteredIntegrationEventHandler(IOutboxIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserRegisteredIntegrationEvent(notification.Email);

            _publisher.Publish(integrationEvent);

            return Task.CompletedTask;
        }
    }
}
