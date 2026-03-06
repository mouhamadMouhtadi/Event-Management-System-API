using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Specifications.EventsSpec
{
    public class EventWithCountSpec : BaseSpecification<Event, int>
    {
        public EventWithCountSpec(EventSpecParams specParams)
       : base(e =>
           (string.IsNullOrEmpty(specParams.Search) || e.Title.ToLower().Contains(specParams.Search))
           &&
           (!specParams.CategoryId.HasValue || e.CategoryId == specParams.CategoryId)
           &&
           (!specParams.OrganizerId.HasValue || e.OrganizerId == specParams.OrganizerId.ToString()))
        {
        }
    }
}
