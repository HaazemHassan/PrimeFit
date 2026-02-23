using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations.BranchEntities
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.DurationInMonths)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.NumberOfFreezes)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.FreezeDurationInDays)
                .HasDefaultValue(0)
                .IsRequired();

            builder.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Subscriptions)
                .WithOne(s => s.Package)
                .HasForeignKey(s => s.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Package_Price_Positive",
                    $"[{nameof(Package.Price)}] >= 0");

                t.HasCheckConstraint(
                    "CK_Package_DurationInMonths_Positive",
                    $"[{nameof(Package.DurationInMonths)}] > 0");

                t.HasCheckConstraint(
                    "CK_Package_NumberOfFreezes_NonNegative",
                    $"[{nameof(Package.NumberOfFreezes)}] >= 0");

                t.HasCheckConstraint(
                    "CK_Package_FreezeDurationInDays_NonNegative",
                    $"[{nameof(Package.FreezeDurationInDays)}] >= 0");
            });
        }
    }
}
