using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.Configurations
{
    internal class EmployeeRolePermissionConfiguration : IEntityTypeConfiguration<EmployeeRolePermission>
    {
        public void Configure(EntityTypeBuilder<EmployeeRolePermission> builder)
        {
            builder.ToTable("EmployeeRolePermissions", "employees");

            builder.HasKey(rp => rp.Id);

            builder.Property(rp => rp.Permission)
                   .HasConversion<string>()
                   .HasMaxLength(100);

            builder.HasOne(rp => rp.EmployeeRole)
                   .WithMany(r => r.Permissions)
                   .HasForeignKey(rp => rp.EmployeeRoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rp => new { rp.EmployeeRoleId, rp.Permission })
                   .IsUnique();
        }
    }
}
