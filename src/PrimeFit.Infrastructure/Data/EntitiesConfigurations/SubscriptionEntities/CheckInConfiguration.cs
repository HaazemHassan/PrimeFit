using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations.SubscriptionEntities
{
    internal class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
    {
        public void Configure(EntityTypeBuilder<CheckIn> builder)
        {
            builder.ToTable("CheckIns", "branches");

            builder.HasKey(c => c.Id);

            builder.HasOne<DomainUser>()
                   .WithMany()
                   .HasForeignKey(c => c.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Branch>()
                   .WithMany()
                   .HasForeignKey(c => c.BranchId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Subscription>()
                   .WithMany(s => s.CheckIns)
                   .HasForeignKey(c => c.SubscriptionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
