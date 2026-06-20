using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.Configurations.SubscriptionEntities
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.PackageId)
                .IsRequired();

            builder.Property(x => x.StripePaymentIntentId)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(PaymentTransactionStatus.Pending);

            builder.Property(x => x.PaidAt);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Package)
                .WithMany(p => p.PaymentTransactions)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.StripePaymentIntentId)
                .IsUnique()
                .HasDatabaseName("IX_PaymentTransactions_StripePaymentIntentId");

            builder.HasIndex(x => new { x.UserId, x.Status })
                .HasDatabaseName("IX_PaymentTransactions_User_Status");

            builder.ToTable("PaymentTransactions", "payments", t =>
            {
                t.HasCheckConstraint(
                    "CK_PaymentTransaction_Amount_NonNegative",
                    $"[{nameof(PaymentTransaction.Amount)}] >= 0");
            });
        }
    }
}
