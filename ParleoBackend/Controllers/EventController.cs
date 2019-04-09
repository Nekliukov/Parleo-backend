using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Filters;
using ParleoBackend.ViewModels.Pages;
using System;
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
        [Authorize]
        public async Task<ActionResult> AddEventParticipant(
            Guid eventId,
            Guid userId)
        {
            var result = await _service.AddEventParticipant(eventId, userId);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEventsPageAsync(
            [FromQuery] EventFilterViewModel eventFilter)
        {
            var result = await _service.GetEventsPageAsync(
                _mapper.Map<EventFilterModel>(eventFilter));

            return Ok(_mapper.Map<PageViewModel<EventViewModel>>(result));
        }

        [HttpGet("{eventId}")]
        [Authorize]
        public async Task<ActionResult> GetEventAsync(Guid eventId)
        {
            var foundEvent = await _service.GetEventAsync(eventId);
            return Ok(_mapper.Map<EventViewModel>(foundEvent));
        }

        [HttpGet("{eventId}/participants")]
        [Authorize]
        public async Task<ActionResult> GetParticipantsPageAsync(
            Guid eventId,
            [FromQuery] PageRequestViewModel pageRequest)
        {
            var participants = await _service.GetParticipantsPageAsync(
                eventId, _mapper.Map<PageRequestModel>(pageRequest));

            return Ok(_mapper.Map<PageViewModel<UserViewModel>>(participants));
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult> CreateEventAsync(
            [FromQuery] CreateOrUpdateEventViewModel entity)
        {
            var createdEvent = await _service.CreateEventAsync(
                _mapper.Map<CreateOrUpdateEventModel>(entity));

            return Ok(_mapper.Map<EventViewModel>(createdEvent));
        }

        [HttpPut("{eventId}")]
        [Authorize]
        public async Task<ActionResult> UpdateEventAsync(
            Guid eventId,
            [FromQuery] CreateOrUpdateEventViewModel entity)
        {
            var result = await _service.UpdateEventAsync(eventId,
                _mapper.Map<CreateOrUpdateEventModel>(entity));

            return Ok();
        }

        [HttpPut("{eventId}/removeParticipant/{userId}")]
        [Authorize]
        public async Task<ActionResult> RemoveEventParticipant(
            Guid eventId, 
            Guid userId)
        {
            bool result = await _service.RemoveEventParticipant(eventId, userId);

            return Ok();
        }
    }
}
