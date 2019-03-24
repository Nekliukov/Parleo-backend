﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Contexts;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Interfaces;

namespace Parleo.DAL.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly EventContext _context;

        public EventsRepository(EventContext context)
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
            return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Event>> GetEventsPageAsync(int offset)
        {
            // Hardcoded 25. add to configure, when it'll be necessary. 
            // This number was approved with front-end
            return await _context.Events.Skip(offset).Take(25)
                .Include(e => e.Participants).ToListAsync();
        }

        public async Task<IEnumerable<UserEvent>> GetParticipantsPageAsync(
            Guid eventId, 
            int offset)
        {
            var participants = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            return  participants.Participants.Skip(offset).Take(25).ToList();
        }

        public async Task<bool> RemoveEventParticipant(Guid eventId, Guid userId)
        {
            Event ev = await _context.Events.FirstOrDefaultAsync(
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

        public async Task<bool> UpdateEventAsync(Event entity)
        {
            _context.Events.Update(entity);
            var result = await _context.SaveChangesAsync();

            return result != 0;
        }
    }
}
