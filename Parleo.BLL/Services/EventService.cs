using System;
using System.Threading.Tasks;
using AutoMapper;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Interfaces;
using Parleo.BLL.Models.Pages;
using Parleo.BLL.Models.Filters;
using Parleo.DAL.Models.Filters;
using Parleo.DAL.Models.Pages;
using System.Linq;

namespace Parleo.BLL.Services
{
    public class EventService : IEventService
    {
        private readonly IEventsRepository _repository;
        private readonly IMapper _mapper;

        public EventService(IEventsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> AddEventParticipant(Guid eventId, Guid userId)
        {
            return await _repository.AddEventParticipant(eventId, userId);       
        }

        public async Task<EventModel> CreateEventAsync(
            CreateOrUpdateEventModel entity)
        {
            Event createdEvent = await _repository.CreateEventAsync(
                _mapper.Map<Event>(entity));

            return _mapper.Map<EventModel>(createdEvent);
        }

        public async Task<EventModel> GetEventAsync(Guid id)
        {
            Event foundEvent = await _repository.GetEventAsync(id);

            return _mapper.Map<EventModel>(foundEvent);
        }

        public async Task<PageModel<EventModel>> GetEventsPageAsync(
            EventFilterModel pageRequest)
        {
            var eventPageModels = await _repository.GetEventsPageAsync(
                _mapper.Map<EventFilter>(pageRequest));

            return _mapper.Map<PageModel<EventModel>>(eventPageModels);
        }

        public async Task<PageModel<UserModel>> GetParticipantsPageAsync(
            Guid eventId, PageRequestModel pageRequest)
        {
            var participantsPageModel = await _repository.GetParticipantsPageAsync(
                eventId, _mapper.Map<PageRequest>(pageRequest));

            var mappedParticipantsPageModel = new PageModel<UserModel>()
            {
                Entities = participantsPageModel.Entities.Select(
                    p => _mapper.Map<UserModel>(p.User)),
                PageNumber = participantsPageModel.PageNumber,
                PageSize = participantsPageModel.PageSize,
                TotalAmount = participantsPageModel.TotalAmount
            };

            return mappedParticipantsPageModel;
        }

        public async Task<bool> RemoveEventParticipant(Guid eventId, Guid userId)
        {
            return await _repository.RemoveEventParticipant(eventId, userId);
        }

        public async Task<bool> UpdateEventAsync(Guid eventId, 
            CreateOrUpdateEventModel entity)
        {
            return await _repository.UpdateEventAsync(
                eventId, _mapper.Map<Event>(entity));
        }
    }
}
