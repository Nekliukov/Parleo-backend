using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Exceptions;
using Parleo.BLL.Extensions;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Pages;
using ParleoBackend.Validators.Common;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Pages;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatService _chatService;
        private readonly IEventService _eventService;

        public ChatsController(IMapperFactory mapperFactory, IChatService chatService, IEventService eventService)
        {
            _mapper = mapperFactory.GetMapper(typeof(WebServices).Name);
            _chatService = chatService;
            _eventService = eventService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateGroupChat([FromBody] CreateGroupChatViewModel groupChat)
        {
            var id = new Guid(User.FindFirst(JwtRegisteredClaimNames.Jti).Value);
            var chatModel = await _chatService.CreateGroupChatAsync(id, _mapper.Map<ChatModel>(groupChat));
            return Ok(_mapper.Map<ChatViewModel>(chatModel));
        }

        [HttpPost("/api/Events/{eventId}/chat")]
        [Authorize]
        public async Task<IActionResult> CreateEventChat([FromQuery] Guid eventId)
        {
            var id = new Guid(User.FindFirst(JwtRegisteredClaimNames.Jti).Value);

            if (!await _eventService.CanUserCreateChat(eventId, id))
            {
                return Forbid(Constants.Errors.NOT_CREATOR);
            }
            return Ok(await _chatService.CreateEventChatAsync(eventId));
        }

        //Do we need it?
        //[HttpGet("/api/Events/{eventId}/chat")]
        //[Authorize]
        //public async Task<IActionResult> GetEventChat([FromQuery] Guid eventId)
        //{
        //    var id = new Guid(User.FindFirst(JwtRegisteredClaimNames.Jti).Value);


        //    return Ok(await _chatService.GetEventChatAsync(id));
        //}

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetChatPage([FromQuery] PageRequestViewModel pageRequest)
        {
            var validator = new PageRequestViewModelValidator();
            ValidationResult validationResult = validator.Validate(pageRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(
                    validationResult.Errors.First().ErrorMessage));
            }
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            var chatsModel = await _chatService.GetChatPageAsync(new Guid(id), _mapper.Map<PageRequestModel>(pageRequest));

            return Ok(_mapper.Map<PageViewModel<ChatViewModel>>(chatsModel));
        }

        [Authorize]
        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChat(Guid chatId)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            var chat = await _chatService.GetChatByIdAsync(chatId, new Guid(id));

            return Ok(_mapper.Map<ChatViewModel>(chat));
        }

        [Authorize]
        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> GetMessagePage(Guid chatId, [FromQuery] PageRequestViewModel pageRequest)
        {
            var validator = new PageRequestViewModelValidator();
            ValidationResult validationResult = validator.Validate(pageRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(
                    validationResult.Errors.First().ErrorMessage));
            }

            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

            var messages = await _chatService.GetMessagePageAsync(chatId, new Guid(id), _mapper.Map<PageRequestModel>(pageRequest));

            return Ok(_mapper.Map<PageViewModel<MessageViewModel>>(messages));
        }

        [Authorize]
        [HttpGet("userId")]
        public async Task<IActionResult> GetChatWithUser([FromQuery] Guid userId)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            var chatModel = await _chatService.GetChatWithUserAsync(new Guid(id), userId);

            return Ok(_mapper.Map<ChatViewModel>(chatModel));
        }
    }
}