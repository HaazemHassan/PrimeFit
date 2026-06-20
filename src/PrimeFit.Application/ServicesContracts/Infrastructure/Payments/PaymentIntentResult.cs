namespace PrimeFit.Application.ServicesContracts.Infrastructure.Payments
{
    public record PaymentIntentResult(string ClientSecret, string PaymentIntentId);
}
