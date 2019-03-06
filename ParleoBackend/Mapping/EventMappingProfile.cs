using AutoMapper;
using DataAssesEvent = Parleo.DAL.Entities.Event;

namespace ParleoBackend.Mapping
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<DataAssesEvent, EventViewModel>();
        }
    }
}
