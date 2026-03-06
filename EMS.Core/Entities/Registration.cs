using EMS.Core.Entities;
using EMS.Core.Entities.Idenitty;

public class Registration : BaseEntity<int>
{

    public string UserId { get; set; }
    public int EventId { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending; // Paid, Pending, Failed
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
    public decimal TotalPrice { get; set; }
    public AppUser User { get; set; }
    public Event Event { get; set; }
}