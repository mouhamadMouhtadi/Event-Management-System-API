using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.DTOs.Events
{
    public class EventRegistrationDto
    {
        public int Id { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentStatus Status { get; set; }
    }

}
