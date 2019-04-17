using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParleoBackend.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatHub(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;
            _mapper = mapper;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public Task SubscribeOnChat(Guid chatId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public Task SubscribeOnChats(ICollection<Guid> chatIds)
        {
            return Task.WhenAll(chatIds.Select(SubscribeOnChat));
        }

        public async Task SendMessage(MessageViewModel message)
        {
            await Clients.Group(message.ChatId.ToString()).SendAsync("receiveMessage", message);
            await _chatService.AddMessagesAsync(message.SenderId,
                new List<MessageModel>() { _mapper.Map<MessageModel>(message) });

        }
    }
}
