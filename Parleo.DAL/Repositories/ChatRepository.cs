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
                .Include(c => c.Event)
                .Include(c => c.Members)
                .ThenInclude(m => m.User)
                .Include(c => c.Messages)
                .Where(c => c.Members.Select(m => m.UserId).Contains(myUserId))
                .FirstOrDefaultAsync(c => c.Id == id);
            if (chat == null)
                return null;
            SetUpUnreadMessages(chat, myUserId);
            var result = new Chat()
            {
                Members = chat.Members,
                Id = chat.Id,
                Name = chat.Name,
                Messages = chat.Messages.OrderByDescending(m => m.CreatedOn).Take(1).ToList(),
                Event = chat.Event,
                CreatorId = chat.CreatorId
            };
            return result;
        }

        public async Task<Page<Chat>> GetChatPageByUserId(Guid userId, PageRequest page)
        {
            var chatPage = await _context.Chat
                .Include(c => c.Event)
                .Include(c => c.Creator)
                .Include(c => c.Event)
                .Include(c => c.Members)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Messages)
                .Select(chat => new 
                    {
                        ChatInfo = chat,
                        Messages = chat.Messages.OrderByDescending(m => m.CreatedOn).Take(1)
                    }
                )
                .Where(c => c.ChatInfo.Members
                    .Select(cu => cu.UserId)
                    .Contains(userId))
                .OrderByDescending(c => c.Messages.Select(m => m.CreatedOn).FirstOrDefault())
                .ToListAsync();

            var chats = chatPage
                .Skip((page.PageNumber - 1) * (page.PageSize ?? PAGE_SIZE))
                .Take(page.PageSize ?? PAGE_SIZE)
                .Select(c => new Chat()
            {
                Id = c.ChatInfo.Id,
                Members = c.ChatInfo.Members,
                Messages = c.Messages.ToList(),
                Name = c.ChatInfo.Name,
                CreatorId = c.ChatInfo.CreatorId,
                Event = c.ChatInfo.Event
            }).ToList();

            SetUpUnreadMessages (chats, userId);

            return new Page<Chat>()
            {
                Entities = chats,
                PageNumber = page.PageNumber,
                PageSize = page.PageSize ?? PAGE_SIZE,
                TotalAmount = chatPage.Count,
                TimeStamp = DateTimeOffset.UtcNow
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
                SetUpUnreadMessages(chat, myUserId);   
            return chat;
        }

        public async Task<Chat> CreateChatAsync(Chat entity)
        {
            _context.Chat.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddMessagesAsync(Guid id, ICollection<Message> messages)
        {
            var chat = _context.Chat
                .Include(c => c.Messages)
                .First(c => c.Id == id);

            foreach(var message in messages)
            {
                chat.Messages.Add(message);
            }
            //chat.Messages.ToList().AddRange(messages);

            await _context.SaveChangesAsync();
        }

        public async Task<Page<Message>> GetMessagePageAsync(Guid chatId, Guid myUserId, PageRequest page)
        {
            var chat = await _context.Chat
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            var chatPage = new Page<Message>
            {
                Entities = chat.Messages.OrderByDescending(m => m.CreatedOn)
                .SkipWhile(m => m.CreatedOn > page.TimeStamp)
                .Skip((page.PageNumber - 1) * (page.PageSize ?? PAGE_SIZE))
                .Take(page.PageSize ?? PAGE_SIZE)
                .ToList(),
                PageNumber = page.PageNumber,
                PageSize = page.PageSize ?? PAGE_SIZE,
                TotalAmount = chat.Messages.Count,
                TimeStamp = DateTimeOffset.UtcNow
            };

            ViewChat(chatId, myUserId);

            return chatPage;
        }

        private void SetUpUnreadMessages(Chat chat, Guid userId)
        {
            if (chat != null)
            {
                var chatUser = chat.Members.First(m => m.UserId == userId);

                chatUser.UnreadMessages = chat.Messages.Count(m => m.CreatedOn > chatUser.TimeStamp);

                _context.SaveChanges();
            }
        }

        private void SetUpUnreadMessages(List<Chat> chatPage, Guid userId)
        {
            foreach (var chat in chatPage)
            {
                var chatUser = chat.Members.First(m => m.UserId == userId);

                chatUser.UnreadMessages = chat.Messages.Where(m => m.SenderId != userId)
                    .Count(m => m.CreatedOn < chatUser.TimeStamp);
            }
            _context.SaveChanges();
        }

        private void ViewChat(Guid chatId, Guid userId)
        {
            var chat = _context.Chat
                .Include(c => c.Members)
                .Include(c => c.Messages)
                .First(c => c.Id == chatId);
            chat.Members
                .First(m => m.UserId == userId)
                .TimeStamp = DateTimeOffset.UtcNow;

            var readMessage = chat.Messages.Where(m => !m.ViewedOn.HasValue && m.SenderId != userId)
                .Select(m => m.ViewedOn = DateTimeOffset.UtcNow).ToList();
        }
    }
}
