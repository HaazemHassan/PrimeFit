using ErrorOr;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class PaymentTransaction : AuditableEntity<int>
    {
        private PaymentTransaction() { }

        public PaymentTransaction(int userId, int packageId, string stripePaymentIntentId, decimal amount, string currency)
        {
            UserId = userId;
            PackageId = packageId;
            StripePaymentIntentId = stripePaymentIntentId;
            Amount = amount;
            Currency = currency;
            Status = PaymentTransactionStatus.Pending;
        }

        // =====================================================================
        // Properties
        // =====================================================================

        public int UserId { get; private set; }
        public int PackageId { get; private set; }
        public string StripePaymentIntentId { get; private set; } = null!;
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = null!;
        public PaymentTransactionStatus Status { get; private set; } = PaymentTransactionStatus.Pending;
        public DateTimeOffset? PaidAt { get; private set; }

        // =====================================================================
        // Navigation Properties
        // =====================================================================

        public DomainUser User { get; private set; } = null!;
        public Package Package { get; private set; } = null!;

        // =====================================================================
        // Domain Methods
        // =====================================================================

        public ErrorOr<Success> MarkAsSucceeded(DateTimeOffset now)
        {
            if (Status != PaymentTransactionStatus.Pending)
                return Error.Validation(description: "Only pending transactions can be marked as succeeded.");

            Status = PaymentTransactionStatus.Succeeded;
            PaidAt = now;
            return Result.Success;
        }

        public ErrorOr<Success> MarkAsFailed()
        {
            if (Status != PaymentTransactionStatus.Pending)
                return Error.Validation(description: "Only pending transactions can be marked as failed.");

            Status = PaymentTransactionStatus.Failed;
            return Result.Success;
        }
    }
}
