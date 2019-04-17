using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Pages;
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
        private readonly ChatHub _chatHub;

        public ChatController(IMapper mapper, IChatService chatService, ChatHub chatHub)
        {
            _mapper = mapper;
            _chatService = chatService;
            _chatHub = chatHub;
        }

        [HttpGet]
        public async Task<ActionResult> GetChatPage(string myUserId, [FromQuery] PageRequestViewModel page)
        {
            //string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

            var chatsModel = await _chatService.GetChatPageAsync(new Guid(myUserId), _mapper.Map<PageRequestModel>(page));

            await _chatHub.SubscribeOnChats(chatsModel.Entities.Select(c => c.Id).ToList());

            return Ok(_mapper.Map<PageViewModel<ChatViewModel>>(chatsModel));
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult> GetChat(Guid chatId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);

            await _chatHub.SubscribeOnChat(chatId);

            return Ok(_mapper.Map<ChatViewModel>(chat));
        }

        [HttpGet("{chatId}/messages")]
        public async Task<ActionResult> GetMessagePage(Guid chatId, [FromQuery] PageRequestViewModel page)
        {
            var messages = await _chatService.GetMessagePageAsync(chatId, _mapper.Map<PageRequestModel>(page));

            return Ok(_mapper.Map<PageViewModel<MessageViewModel>>(messages));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetChatWithUser(string myUserId, [FromQuery] Guid userId)
        {
            //string userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

            var chatModel = await _chatService.GetChatWithUserAsync(new Guid(myUserId), userId);

            await _chatHub.SubscribeOnChat(chatModel.Id);

            return Ok(_mapper.Map<ChatViewModel>(chatModel));
        }

    }
}