using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Infrastructure.Data.Configurations
{
    internal class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("UserNotifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(n => n.NotificationType)
                .IsRequired();

            builder.Property(n => n.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasIndex(n => new { n.UserId, n.IsRead })
                .HasDatabaseName("IX_UserNotifications_UserId_IsRead");

            builder.HasIndex(n => new { n.UserId, n.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("IX_UserNotifications_UserId_CreatedAt_Desc");

            builder.HasIndex(n => n.CreatedAt)
                .HasDatabaseName("IX_UserNotifications_CreatedAt");
        }
    }
}
