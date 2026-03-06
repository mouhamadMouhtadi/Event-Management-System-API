using EMS.Core.Entities;
using EMS.Core.Entities.Idenitty;
using System;

public class PaymentTransaction : BaseEntity<int>
{
    public string UserId { get; set; }
    public int EventId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus PaymentStatus { get; set; } // Success, Failed
    public DateTime TransactionDate { get; set; }

    public AppUser User { get; set; }
    public Event Event { get; set; }
}