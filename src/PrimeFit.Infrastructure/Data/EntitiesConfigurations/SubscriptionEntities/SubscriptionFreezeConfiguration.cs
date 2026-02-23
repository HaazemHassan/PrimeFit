using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations.SubscriptionEntities
{
    public class SubscriptionFreezeConfiguration : IEntityTypeConfiguration<SubscriptionFreeze>
    {
        public void Configure(EntityTypeBuilder<SubscriptionFreeze> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SubscriptionId)
                .IsRequired();

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate);

            builder.Property(x => x.MaxDays)
                .IsRequired();

            builder.HasOne(x => x.Subscription)
                .WithMany(s => s.Freezes)
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(x => !x.Subscription.IsDeleted);

            builder.Ignore(x => x.TotalDays);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_SubscriptionFreeze_MaxDays_Positive",
                    $"[{nameof(SubscriptionFreeze.MaxDays)}] > 0");
            });
        }
    }
}
