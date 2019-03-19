using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.DAL.Interfaces
{
    public interface IEventsRepository
    {
        Task<IEnumerable<Event>> GetEventsPageAsync(int offset);

        // By filters
        Task<Event> GetEventAsync(Guid id);

        Task<IEnumerable<UserEvent>> GetParticipantsPageAsync(Guid eventId, int offset);

        Task<Event> CreateEventAsync(Event entity);

        Task<bool> UpdateEventAsync(Event entity);

        Task<bool> AddEventParticipant(Guid eventId, Guid userId);

        Task<bool> RemoveEventParticipant(Guid eventId, Guid userId);

        // Need to discuss
        // Task<bool> InviteParticipant(Guid eventId, Guid userId);
    }
}