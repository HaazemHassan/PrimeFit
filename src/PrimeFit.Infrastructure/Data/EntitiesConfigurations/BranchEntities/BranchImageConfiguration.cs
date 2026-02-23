using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations.BranchEntities
{
    internal class BranchImageConfiguration : IEntityTypeConfiguration<BranchImage>
    {

        public void Configure(EntityTypeBuilder<BranchImage> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.PublicId)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(i => i.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.BranchId)
                .IsRequired();

            builder.HasOne(i => i.Branch)
                .WithMany()
                .HasForeignKey(i => i.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(i => !i.Branch.IsDeleted);

            builder.HasIndex(i => i.BranchId);
        }
    }
}
