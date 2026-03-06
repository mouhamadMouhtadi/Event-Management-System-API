using EMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.HasKey(pt => pt.Id);
        builder.Property(pt => pt.UserId).IsRequired();
        builder.Property(pt => pt.EventId).IsRequired();
        builder.Property(pt => pt.Amount).IsRequired();
        builder.Property(pt => pt.PaymentStatus).IsRequired();
        builder.Property(pt => pt.TransactionDate).IsRequired();

        builder.HasOne(pt => pt.User)
            .WithMany(u => u.PaymentTransactions)
            .HasForeignKey(pt => pt.UserId);

        builder.HasOne(pt => pt.Event)
            .WithMany(e => e.PaymentTransactions)
            .HasForeignKey(pt => pt.EventId);
    }
}