using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Models.Pages;
using Parleo.DAL.Models.Filters;

namespace Parleo.DAL.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly Contexts.AppContext _context;

        private readonly int _deafultPageSize = 25;

        public EventsRepository(Contexts.AppContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEventParticipant(Guid eventId, Guid userId)
        {
            Event ev = await _context.Events.Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            ev.Participants.Add(new UserEvent
            {
                EventId = eventId,
                UserId = userId
            });

            int result = await _context.SaveChangesAsync();

            return result != 0;
        }

        public async Task<Event> CreateEventAsync(Event entity)
        {
            var ev = _context.Events.Add(entity);
            await _context.SaveChangesAsync();

            return ev.Entity;
        }

        public async Task<Event> GetEventAsync(Guid id)
        {
            return await _context.Events
                .Include(e => e.Language)
                .Include(e => e.Creator)
                .Include(e => e.Participants)
                .ThenInclude(ue => ue.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Page<Event>> GetEventsPageAsync(
            EventFilter eventFilter)
        {
            var events = await _context.Events
                .Where(e => (eventFilter.Languages != null) ?
                    eventFilter.Languages.Contains(e.LanguageId) : true)
                // TODO, need to discus with front
                //.Where(e => (eventFilter.MaxDistance != null) ? true : true)
                //.Where(e => (eventFilter.MinDistance != null) ? true : true)
                .Where(e => (eventFilter.MaxNumberOfParticipants != null) ?
                    e.MaxParticipants <= eventFilter.MaxNumberOfParticipants : true)
                .Where(e => (eventFilter.MinNumberOfParticipants != null) ?
                    e.MaxParticipants >= eventFilter.MaxNumberOfParticipants : true)
                .Where(e => (eventFilter.MaxStartDate != null) ?
                    e.StartTime <= eventFilter.MaxStartDate : true)
                .Where(e => (eventFilter.MinStartDate != null) ?
                    e.StartTime >= eventFilter.MaxStartDate : true)
                .Include(e => e.Creator)
                .Include(e => e.Language)
                .Include(e => e.Participants)
                .ThenInclude(ue => ue.User)
                .ToListAsync();

            int totalAmount = events.Count();

            if (eventFilter.PageSize == null)
            {
                eventFilter.PageSize = _deafultPageSize;
            }

            return new Page<Event>()
            {
                Entities = events
                    .Skip((eventFilter.Page - 1) * eventFilter.PageSize.Value)
                    .Take(eventFilter.PageSize.Value).ToList(),
                PageNumber = eventFilter.Page,
                PageSize = eventFilter.PageSize.Value,
                TotalAmount = totalAmount
            };
        }

        public async Task<Page<UserEvent>> GetParticipantsPageAsync(
            Guid eventId, 
            PageRequest pageRequest)
        {
            var ev = await _context.Events
                .Include(e => e.Participants)
                .ThenInclude(ue => ue.User)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (pageRequest.PageSize == null)
            {
                pageRequest.PageSize = _deafultPageSize;
            }

            int totalAmount = ev.Participants.Count();

            return new Page<UserEvent>()
            {
                Entities = ev.Participants
                    .Skip((pageRequest.Page - 1) * pageRequest.PageSize.Value)
                    .Take(pageRequest.PageSize.Value).ToList(),
                PageNumber = pageRequest.Page,
                PageSize = pageRequest.PageSize.Value,
                TotalAmount = totalAmount
            };
        }

        public async Task<bool> RemoveEventParticipant(Guid eventId, Guid userId)
        {
            Event ev = await _context.Events                
                .Include(e => e.Participants)
                .ThenInclude(ue => ue.User)
                .FirstOrDefaultAsync(
                e => e.Id == eventId);

            if (ev != null)
            {
                UserEvent userEvent = ev.Participants                    
                    .FirstOrDefault(ue => ue.UserId == userId);

                if (userEvent != null)
                {
                    ev.Participants.Remove(userEvent);
                    var result = await _context.SaveChangesAsync();

                    return result != 0;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> UpdateEventAsync(Guid eventId, Event entity)
        {
            Event ev = await _context.Events.SingleOrDefaultAsync(
                e => e.Id == eventId);

            if (ev != null)
            {                
                ev.CreatorId = entity.CreatorId;
                ev.Description = entity.Description;
                ev.EndDate = entity.EndDate;
                ev.IsFinished = entity.IsFinished;
                ev.LanguageId = entity.LanguageId;
                ev.Latitude = entity.Latitude;
                ev.Longitude = entity.Longitude;
                ev.MaxParticipants = entity.MaxParticipants;
                ev.Name = entity.Name;
                ev.StartTime = entity.StartTime;                
            }

            var result = await _context.SaveChangesAsync();

            return result != 0;
        }
    }
}
