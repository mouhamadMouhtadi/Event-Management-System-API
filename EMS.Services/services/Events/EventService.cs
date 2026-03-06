using AutoMapper;
using EMS.Core.DTOs.Events;
using EMS.Core.Repository.Interfaces;
using EMS.Core.Services.Interfaces;
using EMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using EMS.Core.Specifications.EventsSpec;
using EMS.Core.Helper;

namespace EMS.Services.services.Events
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<EventDto>> GetAllAsync(EventSpecParams eventSpec)
        {
            var spec = new EventSpecification(eventSpec);
            var events = await _unitOfWork.Repository<Event, int>().GetAllWithSpecAsync(spec);
           var MappedEvent = _mapper.Map<IEnumerable<EventDto>>(events);
            var countSpec = new EventWithCountSpec(eventSpec);
            var totalItems = await _unitOfWork.Repository<Event, int>().GetCountAsync(countSpec);
            return new PaginationResponse<EventDto>(eventSpec.PageIndex, eventSpec.PageSize, totalItems, MappedEvent.ToList());
        }

        public async Task<EventDto?> GetByIdAsync(int id)
        {
            var events = await _unitOfWork.Repository<Event,int>().GetByIdAsync(id);
            return events == null ? null :  _mapper.Map<EventDto?>(events);
        }
        public async Task<EventDto> CreateAsync(CreateEventRequest request)
        {
            var eventEntity = _mapper.Map<Event>(request);
           await _unitOfWork.Repository<Event, int>().AddAsync(eventEntity);
           await _unitOfWork.CompleteAsync();
            return _mapper.Map<EventDto>(eventEntity);
        }


        public async Task<bool> UpdateAsync(int id, UpdateEventRequest request)
        {
            var repo = _unitOfWork.Repository<Event, int>();
            var eventEntity = await repo.GetByIdAsync(id);
            if (eventEntity == null) return false;

            _mapper.Map(request, eventEntity);
            repo.Update(eventEntity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<Event, int>();
            var eventEntity = await repo.GetByIdAsync(id);
            if (eventEntity == null) return false;

            repo.Delete(eventEntity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

    }
}
