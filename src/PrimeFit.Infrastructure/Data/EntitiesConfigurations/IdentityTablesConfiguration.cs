using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations
{
    internal class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<int>> builder)
        {
            builder.ToTable("AspNetUserClaims", "identity");
        }
    }

    internal class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<int>> builder)
        {
            builder.ToTable("AspNetUserLogins", "identity");
        }
    }

    internal class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            builder.ToTable("AspNetUserRoles", "identity");
        }
    }

    internal class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<int>> builder)
        {
            builder.ToTable("AspNetUserTokens", "identity");
        }
    }

    internal class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<int>> builder)
        {
            builder.ToTable("AspNetRoleClaims", "identity");
        }
    }
}
