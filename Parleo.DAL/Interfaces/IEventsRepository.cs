using Parleo.DAL.Models.Entities;
using Parleo.DAL.Models.Filters;
using Parleo.DAL.Models.Pages;
using System;
using System.Threading.Tasks;

namespace Parleo.DAL.Interfaces
{
    public interface IEventsRepository
    {
        Task<Page<Event>> GetEventsPageAsync(
            EventFilter eventFilter, Location location);
        
        Task<Event> GetEventAsync(Guid id);

        Task<Page<UserEvent>> GetParticipantsPageAsync(
            Guid eventId, PageRequest pageRequest);

        Task<Event> CreateEventAsync(Event entity);

        Task<bool> UpdateEventAsync(Event entity);

        Task<bool> AddEventParticipant(Guid eventId, Guid[] users);

        Task<bool> RemoveEventParticipant(Guid eventId, Guid userId);

        Task InsertImageNameAsync(string imageName, Guid eventId);

        Task<Page<Event>> GetCreatedEvents(
            Guid userId, PageRequest pageRequest);

        Task<Page<Event>> GetAttendedEvents(
            Guid userId, PageRequest pageRequest);

        // Need to discuss
        // Task<bool> InviteParticipant(Guid eventId, Guid userId);
    }
}