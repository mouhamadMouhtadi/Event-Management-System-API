using EMS.Core.DTOs.Events;
using EMS.Core.Helper;
using EMS.Core.Specifications.EventsSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Services.Interfaces
{
    public interface IEventService
    {
        Task<PaginationResponse<EventDto>> GetAllAsync(EventSpecParams productSpec);
        Task<EventDto?> GetByIdAsync(int id);
        Task<EventDto> CreateAsync(CreateEventRequest request);
        Task<bool> UpdateAsync(int id, UpdateEventRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
