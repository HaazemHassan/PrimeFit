using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations.BranchEntities
{
    public class BranchWorkingHourConfiguration : IEntityTypeConfiguration<BranchWorkingHour>
    {
        public void Configure(EntityTypeBuilder<BranchWorkingHour> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Day).IsRequired();

            builder.Property(x => x.OpenTime);
            builder.Property(x => x.CloseTime);

            builder.Property(x => x.IsClosed).HasDefaultValue(false);


            builder.HasOne(x => x.Branch)
                .WithMany(b => b.WorkingHours)
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(x => new { x.BranchId, x.Day }).IsUnique();
        }
    }
}
