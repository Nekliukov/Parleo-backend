using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly IMapper _mapper;

        public EventController(IEventService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPut("{eventId}/addParticipant/{userId}")]
        //[Authorize]
        public async Task<ActionResult> AddEventParticipant(
            Guid eventId, 
            Guid userId)
        {
            var result = await _service.AddEventParticipant(eventId, userId);

            return Ok();
        }

        // TODO
        // Add filter model and implement it (repo -> service -> controller)
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult> GetEventAsync(Guid id)
        {
            await _service.GetEventAsync(id);
            return Ok();
        }

        [HttpGet("{eventId}/page/{offset}")]
        //[Authorize]
        public async Task<ActionResult> GetParticipantsPageAsync(
            Guid eventId, 
            int offset)
        {
            var participants = await _service.GetParticipantsPageAsync(
                eventId, offset);

            return Ok(_mapper.Map<IEnumerable<UserViewModel>>(participants));
        }

        [HttpPost("create")]
        //[Authorize]
        public async Task<ActionResult> CreateEventAsync(UpdateEventViewModel entity)
        {
            var ev = await _service.CreateEventAsync(_mapper.Map<UpdateEventModel>(entity));

            return Ok(_mapper.Map<EventViewModel>(ev));
        }

        [HttpPut("update")]
        //[Authorize]
        public async Task<ActionResult> UpdateEventAsync(UpdateEventViewModel entity)
        {
            var result = await _service.UpdateEventAsync(
                _mapper.Map<EventModel>(entity));

            return Ok();
        }

        [HttpPut("{eventId}/removeParticipant/{userId}")]
        //[Authorize]
        public async Task<ActionResult> RemoveEventParticipant(
            Guid eventId, 
            Guid userId)
        {
            bool result = await _service.RemoveEventParticipant(eventId, userId);

            return Ok();
        }
    }
}
