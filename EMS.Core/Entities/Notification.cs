using EMS.Core.Entities;
using EMS.Core.Entities.Idenitty;
using System;
using System.Globalization;

public class Notification : BaseEntity<int>
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public AppUser  User{ get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }

}