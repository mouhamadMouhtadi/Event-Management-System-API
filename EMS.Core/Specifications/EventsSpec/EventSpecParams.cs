using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Specifications.EventsSpec
{
    public class EventSpecParams : BaseSpecParams
    {
        public int? CategoryId { get; set; }

 
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public Guid? OrganizerId { get; set; }

    }
}
