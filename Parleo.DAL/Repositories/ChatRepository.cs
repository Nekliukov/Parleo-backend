using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Models.Pages;

namespace Parleo.DAL.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppContext _context;

        public ChatRepository(AppContext context)
        {
            _context = context;
        }


        public async Task<Chat> GetChatByIdAsync(Guid id)
        {
            return await _context.Chat.FindAsync(id);
        }

        public async Task<ICollection<Chat>> GetChatByUserId(Guid userId)
        {
            return await _context.Chat
                .Include(c => c.Members)
                .Where(c => c.Members.Select(cu => cu.User)
                .Select(u => u.Id)
                .Contains(userId))
                .ToListAsync();
        }
        public async Task<Page<Chat>> GetChatPageByUserId(Guid userId, PageRequest page)
        {
            var chats = await GetChatByUserId(userId);

            return new Page<Chat>()
            {
                Entities = chats
            };
        }

        public async Task<Chat> GetPrivateChatAsync(Guid myUserId, Guid anotherUserId)
        {
            var chats = await GetChatByUserId(myUserId);

            var chat = chats.Where(c => c.Creator == null)
                .FirstOrDefault(u => u.Id == anotherUserId);

            return chat;
        }

        public async Task<Chat> CreateChatAsync(ICollection<User> members, string chatName)
        {
            var chat = new Chat()
            {
                Members = new List<ChatUser>(),
                Name = chatName,
                Messages = new List<Message>()
            };
            foreach (var member in members)
            {
                chat.Members.Add(new ChatUser()
                {
                    Chat = chat,
                    User = member
                });
            }

            _context.Chat.Add(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task AddMessagesAsync(Guid id, ICollection<Message> messages)
        {
            var chat = await GetChatByIdAsync(id);

            chat.Messages.ToList().AddRange(messages);

            await _context.SaveChangesAsync();
        }

        public async Task<Page<Message>> GetMessagePageAsync(Guid id, PageRequest page)
        {
            var chat = await GetChatByIdAsync(id);
            var chatPage = new Page<Message>();
           chatPage.Entities = chat.Messages
                //.SkipWhile(m => m.Id < lastPageMessageId)  //m => m.CreatedOn < timeStamp
                .Take(20)
                .OrderBy(m => m.CreatedOn)
                .ToList();
           return chatPage;
        }

        public async Task<Message> GetLastMessageAsync(Guid chatId)
        {
            var chat = await GetChatByIdAsync(chatId);

            return chat?.Messages?.OrderBy(m => m.CreatedOn)?.FirstOrDefault();
        }
    }
}
