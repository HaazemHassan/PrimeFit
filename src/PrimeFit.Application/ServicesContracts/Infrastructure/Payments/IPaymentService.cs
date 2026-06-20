using ErrorOr;

namespace PrimeFit.Application.ServicesContracts.Infrastructure.Payments
{
    public interface IPaymentService
    {
        Task<ErrorOr<PaymentIntentResult>> CreatePaymentIntentAsync(
            long amountInSmallestUnit,
            string currency,
            Dictionary<string, string> metadata,
            CancellationToken ct = default);

        StripeWebhookEvent? VerifyAndParseWebhook(string payload, string signature);
    }
}
