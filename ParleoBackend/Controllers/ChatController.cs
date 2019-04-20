using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Exceptions;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Pages;
using ParleoBackend.Contracts;
using ParleoBackend.Hubs;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Pages;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatService _chatService;
        private readonly IChatHub _chatHub;

        public ChatController(IMapper mapper, IChatService chatService, IChatHub chatHub)
        {
            _mapper = mapper;
            _chatService = chatService;
            _chatHub = chatHub;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetChatPage([FromQuery] PageRequestViewModel page)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            var chatsModel = await _chatService.GetChatPageAsync(new Guid(id), _mapper.Map<PageRequestModel>(page));

            try
            {
                await _chatHub.SubscribeOnChats(chatsModel.Entities.Select(c => c.Id).ToList());
            }
            catch (NullReferenceException)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.NO_CONNECION_TO_HUB));
            }

            return Ok(_mapper.Map<PageViewModel<ChatViewModel>>(chatsModel));
        }

        [Authorize]
        [HttpGet("{chatId}")]
        public async Task<ActionResult> GetChat(Guid chatId)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            var chat = await _chatService.GetChatByIdAsync(chatId, new Guid(id));

            try
            {
                await _chatHub.SubscribeOnChat(chatId);
            }
            catch (NullReferenceException)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.NO_CONNECION_TO_HUB));
            }

            return Ok(_mapper.Map<ChatViewModel>(chat));
        }
        
        [Authorize]
        [HttpGet("{chatId}/messages")]
        public async Task<ActionResult> GetMessagePage(Guid chatId, [FromQuery] PageRequestViewModel page)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

            var messages = await _chatService.GetMessagePageAsync(chatId, new Guid(id), _mapper.Map<PageRequestModel>(page));

            return Ok(_mapper.Map<PageViewModel<MessageViewModel>>(messages));
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult> GetChatWithUser([FromQuery] Guid userId)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            var chatModel = await _chatService.GetChatWithUserAsync(new Guid(id), userId);

            try
            {
                await _chatHub.SubscribeOnChat(chatModel.Id);
            }
            catch (NullReferenceException)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.NO_CONNECION_TO_HUB));
            }

            return Ok(_mapper.Map<ChatViewModel>(chatModel));
        }

    }
}