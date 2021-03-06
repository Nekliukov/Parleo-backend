﻿using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parleo.BLL;
using Parleo.BLL.Extensions;
using ParleoBackend.Contracts;

namespace ParleoBackend.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatHub(IMapperFactory mapperFactory, IChatService chatService)
        {
            _mapper = mapperFactory.GetMapper(typeof(WebServices).Name);
            _chatService = chatService;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public Task SubscribeToChat(Guid chatId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public Task SubscribeToChats(ICollection<Guid> chatIds)
        {
            return Task.WhenAll(chatIds.Select(SubscribeToChat));
        }

        public async Task SendMessage(MessageViewModel message)
        {
            await Clients.Group(message.ChatId.ToString()).SendAsync("receiveMessage", message);
            await _chatService.AddMessagesAsync(message.ChatId,
                new List<MessageModel>() { _mapper.Map<MessageModel>(message) });
            //TODO: Create cache for sending message
        }
    }
}
