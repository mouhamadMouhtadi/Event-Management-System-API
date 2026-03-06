using EMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.Location).IsRequired().HasMaxLength(255);
        builder.Property(e => e.DateTime).IsRequired();
        builder.Property(e => e.MaxAttendees).IsRequired();
        builder.Property(e => e.Status)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);
        builder.HasOne(e => e.Category)
       .WithMany(c => c.Events)
       .HasForeignKey(e => e.CategoryId)
       .OnDelete(DeleteBehavior.SetNull);

        builder.Property(e => e.PaymentRequired).IsRequired();

        builder.HasMany(e => e.Registrations)
            .WithOne(r => r.Event)
            .HasForeignKey(r => r.EventId);

        builder.HasMany(e => e.PaymentTransactions)
            .WithOne(pt => pt.Event)
            .HasForeignKey(pt => pt.EventId);

        builder.HasMany(e => e.Notifications)
            .WithOne(n => n.Event)
            .HasForeignKey(n => n.EventId);
        builder.HasOne(e => e.Organizer)
    .WithMany(u => u.OrganizedEvents)
    .HasForeignKey(e => e.OrganizerId)
    .OnDelete(DeleteBehavior.Restrict);

    }
}