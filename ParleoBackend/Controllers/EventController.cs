using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Exceptions;
using Parleo.BLL.Extensions;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using ParleoBackend.Contracts;
using ParleoBackend.Extensions;
using ParleoBackend.Validators;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Filters;
using ParleoBackend.ViewModels.Pages;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly IImageSettings _eventImageSettings;
        private readonly IMapper _mapper;

        public EventController(
            IEventService service,
            IMapperFactory mapperFactory,
            IImageSettings eventImageSettings
        )
        {
            _service = service;
            _eventImageSettings = eventImageSettings;
            _mapper = mapperFactory.GetMapper(typeof(WebServices).Name);
        }

        [HttpPut("{eventId}/addParticipants/{userIds}")]
        [Authorize]
        public async Task<ActionResult> AddEventParticipants(Guid eventId, Guid[] users)
        {
            var result = await _service.AddEventParticipant(eventId, users);

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
            [FromBody] CreateOrUpdateEventViewModel entity)
        {
            var validator = new CrateOrUpdateEventViewModelValidator();
            ValidationResult result = validator.Validate(entity);
            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(result.Errors.First().ErrorMessage));
            }

            var createdEvent = await _service.CreateEventAsync(
                _mapper.Map<CreateOrUpdateEventModel>(entity));

            return Ok(_mapper.Map<EventViewModel>(createdEvent));
        }

        [HttpPut("{eventId}")]
        [Authorize]
        public async Task<ActionResult> UpdateEventAsync(
            Guid eventId,
            [FromBody] CreateOrUpdateEventViewModel entity)
        {
            var validator = new CrateOrUpdateEventViewModelValidator();
            ValidationResult result = validator.Validate(entity);
            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(result.Errors.First().ErrorMessage));
            }

            var updateResult = await _service.UpdateEventAsync(eventId,
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

        [HttpPut("{eventId}/image")]
        [Authorize]
        public async Task<IActionResult> AddUserAccountImage(Guid eventId, IFormFile image)
        {
            if (image == null)
            {
                return BadRequest();
            }

            string eventImagePath = _eventImageSettings.EventDestPath;
            EventModel eventModel = await _service.GetEventAsync(eventId);

            if (eventModel.Image != null)
            {
                System.IO.File.Delete(Path.Combine(eventImagePath, eventModel.Image));
            }

            string accountImageUniqueName = await image.SaveAsync(eventImagePath);

            await _service.InsertEventImageAsync(
                accountImageUniqueName,
                eventId
            );

            return Ok();
        }
    }
}
