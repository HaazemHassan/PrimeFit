namespace PrimeFit.Application.ServicesContracts.Infrastructure.Payments
{
    public record StripeWebhookEvent(string EventId, string PaymentIntentId, string EventType);
}
