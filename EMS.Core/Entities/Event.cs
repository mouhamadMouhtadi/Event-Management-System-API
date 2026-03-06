using EMS.Core.Entities;
using EMS.Core.Entities.Idenitty;
using System;
using System.Collections.Generic;

public class Event : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int MaxAttendees { get; set; }
    public string OrganizerId { get; set; }
    public AppUser Organizer { get; set; }
    public EventStatus Status { get; set; }// Scheduled, Completed, Canceled
    public bool ReminderSent { get; set; } = false;
    public bool PaymentRequired { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public decimal Price { get; set; }

    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}