using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.Configurations
{
    internal class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
    {
        public void Configure(EntityTypeBuilder<EmployeeRole> builder)
        {
            builder.ToTable("EmployeeRoles", "employees");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(r => r.Description)
                   .HasMaxLength(250);

            builder.HasIndex(r => r.Name)
                   .IsUnique();
        }
    }
}
