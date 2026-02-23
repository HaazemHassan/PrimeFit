using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations.BranchEntities
{
    public class BranchReviewConfiguration : IEntityTypeConfiguration<BranchReview>
    {
        public void Configure(EntityTypeBuilder<BranchReview> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.Property(x => x.Comment)
                .HasMaxLength(300);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Branch).WithMany(b => b.Reviews).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(x => new { x.UserId, x.BranchId }).IsUnique();

            builder.HasQueryFilter(x => !x.Branch.IsDeleted);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_BranchReview_Rating_Range",
                    $"[{nameof(BranchReview.Rating)}] >= 1 AND [{nameof(BranchReview.Rating)}] <= 5");
            });

        }
    }
}
