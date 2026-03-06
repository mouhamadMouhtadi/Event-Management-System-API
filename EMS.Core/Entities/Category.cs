using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Entities
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
