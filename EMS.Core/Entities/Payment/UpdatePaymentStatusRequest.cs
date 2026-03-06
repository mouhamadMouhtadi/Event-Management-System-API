using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Entities.Payment
{
    public class UpdatePaymentStatusRequest
    {
        public int RegistrationId { get; set; }
        public string Status { get; set; } // "Success" / "Failed" / "Pending"
    }
}
