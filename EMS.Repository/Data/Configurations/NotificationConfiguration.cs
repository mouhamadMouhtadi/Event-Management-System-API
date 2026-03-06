using EMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.UserId).IsRequired();
        builder.Property(n => n.EventId).IsRequired();
        builder.Property(n => n.Message).IsRequired().HasMaxLength(500);
        builder.Property(n => n.DateTime).IsRequired();

        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.Event)
            .WithMany(e => e.Notifications)
            .HasForeignKey(n => n.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
