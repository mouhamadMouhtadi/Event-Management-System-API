using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.DTOs.Payment
{
    public class PaymentRequest
    {
        public int EntityId { get; set; } 
        public string Currency { get; set; } = "usd";
    }

}
