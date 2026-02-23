using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations.SubscriptionEntities
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.PackageId)
                .IsRequired();

            builder.Property(x => x.BranchId)
                .IsRequired();

            builder.Property(x => x.ActivationDate);

            builder.Property(x => x.EndDate);

            builder.Property(x => x.CancellationDate);

            builder.Property(x => x.PaidAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.AllowedFreezeCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.AllowedFreezeDays)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.DurationInMonths)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Package)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Freezes)
                .WithOne(f => f.Subscription)
                .HasForeignKey(f => f.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Subscription_PaidAmount_NonNegative",
                    $"[{nameof(Subscription.PaidAmount)}] >= 0");

                t.HasCheckConstraint(
                    "CK_Subscription_AllowedFreezeCount_NonNegative",
                    $"[{nameof(Subscription.AllowedFreezeCount)}] >= 0");

                t.HasCheckConstraint(
                    "CK_Subscription_AllowedFreezeDays_NonNegative",
                    $"[{nameof(Subscription.AllowedFreezeDays)}] >= 0");

                t.HasCheckConstraint(
                    "CK_Subscription_DurationInMonths_Positive",
                    $"[{nameof(Subscription.DurationInMonths)}] > 0");
            });
        }
    }
}
