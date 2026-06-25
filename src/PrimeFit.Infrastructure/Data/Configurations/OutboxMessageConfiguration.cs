using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Infrastructure.Data.Outbox;

namespace PrimeFit.Infrastructure.Data.Configurations
{
    internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.OccurredOnUtc)
                .HasFilter("[ProcessedOnUtc] IS NULL");
        }
    }
}
