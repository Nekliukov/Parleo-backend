﻿using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using System;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IEventService
    {
        Task<PageModel<EventModel>> GetEventsPageAsync(
            EventFilterModel eventFilter, UserModel user);
        
        Task<EventModel> GetEventAsync(Guid id);

        Task<PageModel<UserModel>> GetParticipantsPageAsync(
            Guid eventId, PageRequestModel pageRequest);

        Task<EventModel> CreateEventAsync(CreateEventModel entity);

        Task<bool> UpdateEventAsync(Guid eventId, UpdateEventModel entity);

        Task<bool> AddEventParticipant(Guid eventId, Guid[] users);

        Task<bool> RemoveEventParticipant(Guid eventId, Guid userId);

        Task<bool> UpdateEventLocationAsync(Guid eventId, LocationModel location);

        Task InsertEventImageAsync(string imageName, Guid eventId);

        Task<bool> CanParticipate(Guid eventId, Guid[] participants);

        Task<bool> AlreadyParticipate(Guid eventId, Guid[] participants);

        Task<PageModel<EventModel>> GetCreatedEvents(
            Guid userId, PageRequestModel pageRequest);

        Task<PageModel<EventModel>> GetAttendingEvents(
            Guid userId, PageRequestModel pageRequest);

        Task<bool> CanUserCreateChat(Guid EventId, Guid userId);

        // Need to discuss
        // Task<bool> InviteParticipant(Guid eventId, Guid userId);
    }
}