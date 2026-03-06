using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Entities.Idenitty
{
    public class AppUser:IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }

        // Navigation properties
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
    }
}
