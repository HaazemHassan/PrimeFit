namespace PrimeFit.Application.Common.Messaging
{
    public interface IOutboxIntegrationEventPublisher
    {
        void Publish(IIntegrationEvent integrationEvent);
    }
}
