using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrimeFit.Application.ServicesContracts.Infrastructure.Payments;
using PrimeFit.Domain.Common.Constants;
using Stripe;

namespace PrimeFit.Infrastructure.Payments
{
    internal sealed class StripePaymentService : IPaymentService
    {
        private readonly StripeOptions _options;
        private readonly ILogger<StripePaymentService> _logger;

        public StripePaymentService(
            IOptions<StripeOptions> options,
            ILogger<StripePaymentService> logger)
        {
            _options = options.Value;
            _logger = logger;

            StripeConfiguration.ApiKey = _options.SecretKey;
        }

        public async Task<ErrorOr<PaymentIntentResult>> CreatePaymentIntentAsync(
            long amountInSmallestUnit,
            string currency,
            Dictionary<string, string> metadata,
            CancellationToken ct = default)
        {
            try
            {
                var paymentIntentService = new PaymentIntentService();

                var options = new PaymentIntentCreateOptions
                {
                    Amount = amountInSmallestUnit,
                    Currency = currency,
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                        AllowRedirects = "never"

                    },
                    Metadata = metadata
                };

                var paymentIntent = await paymentIntentService.CreateAsync(options, cancellationToken: ct);

                _logger.LogInformation(
                    "PaymentIntent created successfully. Id: {PaymentIntentId}, Amount: {Amount} {Currency}",
                    paymentIntent.Id, amountInSmallestUnit, currency);

                return new PaymentIntentResult(paymentIntent.ClientSecret, paymentIntent.Id);
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex,
                    "Failed to create PaymentIntent. Error: {ErrorCode}, Message: {Message}",
                    ex.StripeError?.Code, ex.Message);

                return Error.Failure(
                    ErrorCodes.Payment.PaymentIntentCreationFailed,
                    "Failed to create payment intent.");
            }
        }

        public StripeWebhookEvent? VerifyAndParseWebhook(string payload, string signature)
        {
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    payload, signature, _options.WebhookSecret);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                if (paymentIntent is null)
                {
                    _logger.LogWarning(
                        "Webhook event {EventId} does not contain a PaymentIntent.", stripeEvent.Id);
                    return null;
                }

                return new StripeWebhookEvent(
                    stripeEvent.Id,
                    paymentIntent.Id,
                    stripeEvent.Type);
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Failed to construct Stripe webhook event. Signature verification failed.");
                return null;
            }
        }
    }
}
