using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.EntitiesConfigurations
{
    internal class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
    {
        public void Configure(EntityTypeBuilder<VerificationCode> builder)
        {
            builder.ToTable("VerificationCodes", "auth");
        }
    }
}
