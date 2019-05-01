using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Parleo.BLL.Extensions;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Pages;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Models.Pages;

namespace Parleo.BLL.Services
{
    class ChatService : IChatService
    {
        private readonly IAccountService _accountService;
        private readonly IChatRepository _chatRepository;
        private readonly IChatHelper _chatHelper;
        private readonly IMapper _mapper;

        public ChatService(
            IAccountService accountService, 
            IChatRepository chatRepository, 
            IMapperFactory mapperFactory, 
            IChatHelper chatHelper)
        {
            _accountService = accountService;
            _chatRepository = chatRepository;
            _chatHelper = chatHelper;
            _mapper = mapperFactory.GetMapper(typeof(BLServices).Name); ;
        }
        public async Task<ChatModel> GetChatWithUserAsync(Guid myId, Guid anotherUserId)
        {
            var chat = await _chatRepository.GetPrivateChatAsync(myId, anotherUserId);

            if (chat == null)
            {
                var anotherUser = await _accountService.GetUserByIdAsync(anotherUserId);

                chat = await _chatRepository.CreateChatAsync(new List<Guid>()
                {
                    myId,
                    anotherUser.Id
                });
            }

            var chatModel = _mapper.Map<ChatModel>(chat);
            _chatHelper.GetChatDefinition(chatModel, myId);
            return chatModel;
        }

        public async Task<ChatModel> GetChatByIdAsync(Guid chatId, Guid myUserId)
        {
            var chat = await _chatRepository.GetChatByIdAsync(chatId, myUserId);
            if (chat == null)
            {
                throw new AppException(ErrorType.InvalidId);
            }

            var chatModel = _mapper.Map<ChatModel>(chat);
            _chatHelper.GetChatDefinition(chatModel, myUserId);
            return chatModel;
        }

        public async Task<PageModel<ChatModel>> GetChatPageAsync(Guid userId, PageRequestModel pageRequest)
        {
            var page = await _chatRepository.GetChatPageByUserId(userId, _mapper.Map<PageRequest>(pageRequest));

            var chatModel = _mapper.Map<PageModel<ChatModel>>(page);
            _chatHelper.GetChatDefinition(chatModel.Entities, userId);
            return chatModel;
        }

        public async Task AddMessagesAsync(Guid chatId, ICollection<MessageModel> messages)
        {
            await _chatRepository.AddMessagesAsync(chatId, _mapper.Map<ICollection<Message>>(messages));
        }

        public async Task<PageModel<MessageModel>> GetMessagePageAsync(Guid userId, Guid myUserId,
            PageRequestModel pageRequest)
        {
            var page = await _chatRepository.GetMessagePageAsync(userId, myUserId, _mapper.Map<PageRequest>(pageRequest));

            return _mapper.Map<PageModel<MessageModel>>(page);
        }
    }
}
