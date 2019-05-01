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
using Parleo.BLL.Extensions;

namespace Parleo.BLL.Services
{
    public class EventService : IEventService
    {
        private readonly IEventsRepository _repository;
        private readonly IMapper _mapper;

        public EventService(IEventsRepository repository, IMapperFactory mapperFactory)
        {
            _repository = repository;
            _mapper = mapperFactory.GetMapper(typeof(BLServices).Name);
        }

        public async Task<bool> AddEventParticipant(Guid eventId, Guid[] users)
        {
            return await _repository.AddEventParticipant(eventId, users);
        }

        public async Task<EventModel> CreateEventAsync(
            CreateEventModel entity)
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
            EventFilterModel pageRequest, UserModel user)
        {
            var location = new LocationModel();
            (location.Latitude, location.Longitude) = (user.Latitude, user.Longitude);

            var eventPageModels = await _repository.GetEventsPageAsync(
                _mapper.Map<EventFilter>(pageRequest), _mapper.Map<Location>(location));

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
            UpdateEventModel entity)
        {
            var updatingEvent = await _repository.GetEventAsync(eventId);

            _mapper.Map(entity, updatingEvent);

            return await _repository.UpdateEventAsync(updatingEvent);
        }

        public async Task InsertEventImageAsync(string imageName, Guid eventId)
            => await _repository.InsertImageNameAsync(imageName, eventId);

        public async Task<bool> UpdateEventLocationAsync(Guid eventId,
            LocationModel location)
        {
            var updatingEvent = await _repository.GetEventAsync(eventId);

            if (updatingEvent == null)
            {
                return false;
            }

            (updatingEvent.Latitude, updatingEvent.Longitude) =
                (location.Latitude, location.Longitude);

            return await _repository.UpdateEventAsync(updatingEvent);
        }

        public async Task<bool> CanParticipate(Guid eventId, Guid[] participants)
        {
            Event targetEvent = await _repository.GetEventAsync(eventId);

            if (targetEvent.Participants.Count() + participants.Count() > 
                    targetEvent.MaxParticipants)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> AlreadyParticipate(Guid eventId, Guid[] participants)
        {
            Event targetEvent = await _repository.GetEventAsync(eventId);

            if (targetEvent.Participants.Any(p => participants.Any(id => p.UserId == id)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
