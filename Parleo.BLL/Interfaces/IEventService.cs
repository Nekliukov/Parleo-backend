using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using System;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IEventService
    {
        Task<PageModel<EventModel>> GetEventsPageAsync(
            EventFilterModel eventFilter);
        
        Task<EventModel> GetEventAsync(Guid id);

        Task<PageModel<UserModel>> GetParticipantsPageAsync(
            Guid eventId, PageRequestModel pageRequest);

        Task<EventModel> CreateEventAsync(UpdateEventModel entity);

        Task<bool> UpdateEventAsync(Guid eventId, UpdateEventModel entity);

        Task<bool> AddEventParticipant(Guid eventId, Guid userId);

        Task<bool> RemoveEventParticipant(Guid eventId, Guid userId);

        // Need to discuss
        // Task<bool> InviteParticipant(Guid eventId, Guid userId);
    }
}