using AutoMapper;
using ParleoBackend.ViewModels;
using DataAccessEvent = Parleo.DAL.Entities.Event;

namespace ParleoBackend.Mapping
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<DataAccessEvent, EventViewModel>();
        }
    }
}
