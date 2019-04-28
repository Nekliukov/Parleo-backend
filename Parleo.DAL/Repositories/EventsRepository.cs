using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Models.Pages;
using Parleo.DAL.Models.Filters;
using Parleo.DAL.Helpers;

namespace Parleo.DAL.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly AppContext _context;

        private readonly int _defaultPageSize = 25;

        public EventsRepository(AppContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEventParticipant(Guid eventId, Guid[] users)
        {
            Event targetEvent = await _context.Event
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            foreach (Guid userId in users)
            {
                targetEvent.Participants.Add(new UserEvent
                {
                    EventId = eventId,
                    UserId = userId
                });
            }

            int result = await _context.SaveChangesAsync();

            return result != 0;
        }

        public async Task<Event> CreateEventAsync(Event entity)
        {
            var createdEvent = _context.Event.Add(entity);
            await _context.SaveChangesAsync();

            return createdEvent.Entity;
        }

        public async Task<Event> GetEventAsync(Guid id)
        {
            return await _context.Event
                .Include(e => e.Language)
                .Include(e => e.Creator)
                .Include(e => e.Participants)
                .ThenInclude(ue => ue.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Page<Event>> GetEventsPageAsync(
            EventFilter eventFilter, Location location)
        {
            double latitude = (double)location.Latitude,
                   longtitude = (double)location.Longitude;
            var events = await _context.Event
                .Where(e => (eventFilter.Languages != null && 
                        eventFilter.Languages.Count() != 0) ?
                    eventFilter.Languages.Contains(e.LanguageCode) : true)
                .Where(e => (eventFilter.MaxDistance != null) ?
                    LocationHelper.GetDistanceBetween((double)e.Longitude, (double)e.Latitude,
                    longtitude, latitude) <= eventFilter.MaxDistance : true)
                .Where(e => (eventFilter.MaxNumberOfParticipants != null) ?
                    e.MaxParticipants <= eventFilter.MaxNumberOfParticipants : true)
                .Where(e => (eventFilter.MinNumberOfParticipants != null) ?
                    e.MaxParticipants >= eventFilter.MinNumberOfParticipants : true)
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
                eventFilter.PageSize = _defaultPageSize;
            }

            return new Page<Event>()
            {
                Entities = events.OrderBy(e => e.StartTime)
                    .SkipWhile(m => m.StartTime > eventFilter.TimeStamp)
                    .Skip((eventFilter.Page - 1) * eventFilter.PageSize.Value)
                    .Take(eventFilter.PageSize.Value).ToList(),
                PageNumber = eventFilter.Page,
                PageSize = eventFilter.PageSize.Value,
                TotalAmount = totalAmount,
                TimeStamp = DateTimeOffset.UtcNow
            };
        }

        public async Task<Page<UserEvent>> GetParticipantsPageAsync(
            Guid eventId, 
            PageRequest pageRequest)
        {
            var targetEvent = await _context.Event
                .Include(e => e.Participants)
                .ThenInclude(ue => ue.User)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (pageRequest.PageSize == null)
            {
                pageRequest.PageSize = _defaultPageSize;
            }

            int totalAmount = targetEvent.Participants.Count();

            return new Page<UserEvent>()
            {
                Entities = targetEvent.Participants
                    .Skip((pageRequest.Page - 1) * pageRequest.PageSize.Value)
                    .Take(pageRequest.PageSize.Value).ToList(),
                PageNumber = pageRequest.Page,
                PageSize = pageRequest.PageSize.Value,
                TotalAmount = totalAmount,
                TimeStamp = DateTimeOffset.UtcNow
            };
        }

        public async Task<bool> RemoveEventParticipant(Guid eventId, Guid userId)
        {
            Event targetEvet = await _context.Event
                .Include(e => e.Participants)
                .ThenInclude(ue => ue.User)
                .FirstOrDefaultAsync(
                e => e.Id == eventId);

            if (targetEvet != null)
            {
                UserEvent userEvent = targetEvet.Participants
                    .FirstOrDefault(ue => ue.UserId == userId);

                if (userEvent != null)
                {
                    targetEvet.Participants.Remove(userEvent);
                    var result = await _context.SaveChangesAsync();

                    return result != 0;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> UpdateEventAsync(Event entity)
        {
            _context.Event.Update(entity);

            var result = await _context.SaveChangesAsync();

            return result != 0;
        }

        public async Task InsertImageNameAsync(string imageName, Guid eventId)
        {
            Event updatedEvent = new Event()
            {
                Id = eventId,
                Image = imageName
            };
            _context.Entry(updatedEvent).Property(x => x.Image).IsModified = true;
            await _context.SaveChangesAsync();
        }
    }
}
