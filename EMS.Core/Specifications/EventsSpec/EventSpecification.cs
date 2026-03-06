using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Specifications.EventsSpec
{
    public class EventSpecification : BaseSpecification<Event, int>
    {
        public EventSpecification(EventSpecParams specParams) : base(
            e =>
                (string.IsNullOrEmpty(specParams.Search) ||
                 e.Title.ToLower().Contains(specParams.Search) ||
                 e.Description.ToLower().Contains(specParams.Search))
                &&
                (!specParams.CategoryId.HasValue || e.CategoryId == specParams.CategoryId)
                &&
                (!specParams.OrganizerId.HasValue || e.OrganizerId == specParams.OrganizerId.ToString())
        )
        {
            // Sorting
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "dateAsc":
                        AddOrderBy(e => e.DateTime);
                        break;
                    case "dateDesc":
                        AddOrderByDesc(e => e.DateTime);
                        break;
                    case "titleAsc":
                        AddOrderBy(e => e.Title);
                        break;
                    case "titleDesc":
                        AddOrderByDesc(e => e.Title);
                        break;
                    default:
                        AddOrderBy(e => e.DateTime);
                        break;
                }
            }
            else
            {
                AddOrderBy(e => e.DateTime); 
            }

   
            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

      
            Includes.Add(e => e.Category);
            Includes.Add(e => e.Organizer);
        }
    }

}
