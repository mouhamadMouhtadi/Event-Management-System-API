using AutoMapper;
using EMS.Core.DTOs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Mapping.Events
{
    public class EventProfile : Profile
    {
        public EventProfile() {
            CreateMap<Event, EventDto>()
                     .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                      .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.DateTime))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
            CreateMap<CreateEventRequest, Event>();
            CreateMap<UpdateEventRequest, Event>();

        }
    }
}
