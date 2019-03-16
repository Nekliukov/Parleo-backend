using Parleo.BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventModel>> GetEventsPageAsync(int offset);

        // By filters
        Task<EventModel> GetEventAsync(Guid id);

        Task<IEnumerable<UserModel>> GetParticipantsPageAsync(Guid eventId, int offset);

        Task<EventModel> CreateEventAsync(EventModel entity);

        Task<bool> UpdateEventAsync(EventModel entity);

        Task<bool> AddEventParticipant(Guid eventId, Guid userId);

        Task<bool> RemoveEventParticipant(Guid eventId, Guid userId);

        // Need to discuss
        // Task<bool> InviteParticipant(Guid eventId, Guid userId);
    }
}