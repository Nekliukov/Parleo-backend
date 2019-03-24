using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Interfaces;

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

        public async Task<EventModel> CreateEventAsync(UpdateEventModel entity)
        {
            Event ev = await _repository.CreateEventAsync(_mapper.Map<Event>(entity));
            return _mapper.Map<EventModel>(ev);
        }

        public Task<EventModel> GetEventAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EventModel>> GetEventsPageAsync(int offset)
        {
            var eventModels = await _repository.GetEventsPageAsync(offset);

            return _mapper.Map<IEnumerable<EventModel>>(eventModels);
        }

        public async Task<IEnumerable<UserModel>> GetParticipantsPageAsync(Guid eventId, int offset)
        {
            var participants = await _repository.GetParticipantsPageAsync(eventId, offset);

            return participants.Select(p => _mapper.Map<UserModel>(p.User));
        }

        public async Task<bool> RemoveEventParticipant(Guid eventId, Guid userId)
        {
            return await _repository.RemoveEventParticipant(eventId, userId);
        }

        public async Task<bool> UpdateEventAsync(EventModel entity)
        {
            return await _repository.UpdateEventAsync(_mapper.Map<Event>(entity));
        }
    }
}
