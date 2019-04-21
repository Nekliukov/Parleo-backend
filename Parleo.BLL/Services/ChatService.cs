using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ChatService(IAccountService accountService, IChatRepository chatRepository, IMapper mapper)
        {
            _accountService = accountService;
            _chatRepository = chatRepository;
            _mapper = mapper;
        }
        public async Task<ChatModel> GetChatWithUserAsync(Guid myId, Guid anotherUserId)
        {
            var chat = await _chatRepository.GetPrivateChatAsync(myId, anotherUserId);

            if (chat == null)
            {
                var user = await _accountService.GetUserByIdAsync(myId);
                var anotherUser = await _accountService.GetUserByIdAsync(anotherUserId);

                chat = await _chatRepository.CreateChatAsync(new List<User>()
                {
                    _mapper.Map<User>(user),
                    _mapper.Map<User>(anotherUser),

                }, anotherUser.Name);
            }
            return _mapper.Map<ChatModel>(chat);
        }

        public async Task<ChatModel> GetChatByIdAsync(Guid chatId, Guid myUserId)
        {
            var chat = await _chatRepository.GetChatByIdAsync(chatId, myUserId);
            if (chat == null)
            {
                throw new AppException(ErrorType.InvalidId);
            }
            return _mapper.Map<ChatModel>(chat);
        }

        public async Task<PageModel<ChatModel>> GetChatPageAsync(Guid userId, PageRequestModel pageRequest)
        {
            var page = await _chatRepository.GetChatPageByUserId(userId, _mapper.Map<PageRequest>(pageRequest));

            return _mapper.Map<PageModel<ChatModel>>(page);
        }

        public async Task AddMessagesAsync(Guid userId, ICollection<MessageModel> messages)
        {
            await _chatRepository.AddMessagesAsync(userId, _mapper.Map<ICollection<Message>>(messages));
        }

        public async Task<PageModel<MessageModel>> GetMessagePageAsync(Guid userId, Guid myUserId,
            PageRequestModel pageRequest)
        {
            var page = await _chatRepository.GetMessagePageAsync(userId, myUserId, _mapper.Map<PageRequest>(pageRequest));

            return _mapper.Map<PageModel<MessageModel>>(page);
        }
    }
}
