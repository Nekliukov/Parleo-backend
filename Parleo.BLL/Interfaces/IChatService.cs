using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Pages;

namespace Parleo.BLL.Interfaces
{
    public interface IChatService
    {
        Task<ChatModel> GetChatWithUserAsync(Guid myId, Guid anotherUserId);

        Task<ChatModel> GetChatByIdAsync(Guid chatId, Guid myUserId);

        Task<PageModel<ChatModel>> GetChatPageAsync(Guid userId, PageRequestModel pageRequest);

        Task AddMessagesAsync(Guid userId, ICollection<MessageModel> messages);
        
        Task<PageModel<MessageModel>> GetMessagePageAsync(
            Guid id, Guid myUserId, PageRequestModel pageRequest);
    }
}
