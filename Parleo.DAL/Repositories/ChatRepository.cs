using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading;
using Microsoft.EntityFrameworkCore.Storage;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Models.Pages;

namespace Parleo.DAL.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppContext _context;
        private const int PAGE_SIZE = 20;

        public ChatRepository(AppContext context)
        {
            _context = context;
        }


        public async Task<Chat> GetChatByIdAsync(Guid id, Guid myUserId)
        {
            var chat = await _context.Chat
                .Include(c => c.Members)
                .ThenInclude(m => m.User)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);
            SetUpUnreadMessages(ref chat, myUserId);
            var result = new Chat()
            {
                Creator = chat.Creator,
                Members = chat.Members,
                Id = chat.Id,
                Name = chat.Name,
                Messages = chat.Messages.OrderBy(m => m.CreatedOn).Take(1).ToList()
            };
            return result;
        }

        public async Task<Page<Chat>> GetChatPageByUserId(Guid userId, PageRequest page)
        {
            var chatPage = await _context.Chat
                .Include(c => c.Members)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Messages)
                .Select(chat => new 
                    {
                        ChatInfo = chat,
                        Messages = chat.Messages.OrderBy(m => m.CreatedOn).Take(1)
                    }
                )
                .Where(c => c.ChatInfo.Members
                    .Select(cu => cu.UserId)
                    .Contains(userId))
                .OrderBy(c => c.Messages.Select(m => m.CreatedOn).FirstOrDefault())
                .ToListAsync();

            var chats = chatPage
                .Skip((page.Page - 1) * (page.PageSize ?? PAGE_SIZE))
                .Take(page.PageSize ?? PAGE_SIZE)
                .Select(c => new Chat()
            {
                Creator = c.ChatInfo.Creator,
                Id = c.ChatInfo.Id,
                Members = c.ChatInfo.Members,
                Messages = c.Messages.ToList(),
                Name = c.ChatInfo.Name
            }).ToList();

            SetUpUnreadMessages (ref chats, userId);

            return new Page<Chat>()
            {
                Entities = chats,
                PageNumber = page.Page,
                PageSize = page.PageSize ?? PAGE_SIZE,
                TotalAmount = chatPage.Count
            };
        }

        public async Task<Chat> GetPrivateChatAsync(Guid myUserId, Guid anotherUserId)
        {
            var chat = await _context.Chat
                .Include(c => c.Members)
                .ThenInclude(m => m.User)
                .Include(c => c.Messages)
                .Where(c => c.Members
                    .Select(cu => cu.UserId)
                    .Contains(myUserId))
                .Where(c => c.Creator == null)
                .FirstOrDefaultAsync(u => u.Members
                    .Select(m => m.UserId)
                    .Contains(anotherUserId));
            if (chat != null) 
                SetUpUnreadMessages(ref chat, myUserId);   
            return chat;
        }

        public async Task<Chat> CreateChatAsync(ICollection<User> members, string chatName, User creator = null)
        {
            var chat = new Chat()
            {
                Members = new List<ChatUser>(),
                Name = chatName,
                Messages = new List<Message>(),
                Creator = creator
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
            var chat = _context.Chat
                .Include(c => c.Messages)
                .First(c => c.Id == id);

            chat.Messages.ToList().AddRange(messages);

            await _context.SaveChangesAsync();
        }

        public async Task<Page<Message>> GetMessagePageAsync(Guid chatId, Guid myUserId, PageRequest page)
        {
            var chat = await _context.Chat
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            var chatPage = new Page<Message>
            {
                Entities = chat.Messages.OrderBy(m => m.CreatedOn)
                .SkipWhile(m => m.CreatedOn > page.TimeStamp)
                .Skip((page.Page - 1) * (page.PageSize ?? PAGE_SIZE))
                .Take(page.PageSize ?? PAGE_SIZE)
                .ToList(),
                PageNumber = page.Page,
                PageSize = page.PageSize ?? PAGE_SIZE,
                TotalAmount = chat.Messages.Count
            };

            ViewChat(chatId, myUserId);

            return chatPage;
        }

        private void SetUpUnreadMessages(ref Chat chat, Guid userId)
        {
            if (chat != null)
            {
                var chatUser = chat.Members.First(m => m.UserId == userId);

                chatUser.UnreadMessages = chat.Messages.Count(m => m.CreatedOn > chatUser.TimeStamp);

                _context.SaveChanges();
            }
        }

        private void SetUpUnreadMessages(ref List<Chat> chatPage, Guid userId)
        {
            foreach (var chat in chatPage)
            {
                var chatUser = chat.Members.First(m => m.UserId == userId);

                chatUser.UnreadMessages = chat.Messages.Count(m => m.CreatedOn > chatUser.TimeStamp);
            }
            _context.SaveChanges();
        }

        private void ViewChat(Guid chatId, Guid userId)
        {
            _context.Chat
                .Include(c => c.Members)
                .First(c => c.Id == chatId)
                .Members
                .First(m => m.UserId == userId)
                .TimeStamp = new DateTimeOffset();
        }
    }
}
