using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Payments.Queries.GetPaymentTransactionDetails
{
    public class GetPaymentTransactionDetailsQueryResponse
    {
        public int TransactionId { get; set; }
        public PaymentTransactionStatus Status { get; set; }
        public DateTimeOffset? PaidAt { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public int PackageId { get; set; }
    }
}
