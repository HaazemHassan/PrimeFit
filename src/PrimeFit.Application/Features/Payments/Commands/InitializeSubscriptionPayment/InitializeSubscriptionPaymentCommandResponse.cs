namespace PrimeFit.Application.Features.Payments.Commands.InitializeSubscriptionPayment
{
    public class InitializeSubscriptionPaymentCommandResponse
    {
        public string ClientSecret { get; set; } = null!;
        public int PaymentTransactionId { get; set; }
    }
}
