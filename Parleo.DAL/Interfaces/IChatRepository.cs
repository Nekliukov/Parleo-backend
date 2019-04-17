using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Models.Pages;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Parleo.DAL.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetChatByIdAsync(Guid id);

        Task<ICollection<Chat>> GetChatByUserId(Guid userId);

        Task<Page<Chat>> GetChatPageByUserId(Guid userId, PageRequest page);

        Task<Chat> GetPrivateChatAsync(Guid myUserId, Guid anotherUserId);

        Task<Chat> CreateChatAsync(ICollection<User> members, string chatName);

        Task AddMessagesAsync(Guid id, ICollection<Message> messages);

        Task<Page<Message>> GetMessagePageAsync(Guid id, PageRequest page);

        Task<Message> GetLastMessageAsync(Guid chatId);


    }
}
