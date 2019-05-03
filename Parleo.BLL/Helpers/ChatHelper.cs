using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;

namespace Parleo.BLL.Helpers
{
    public class ChatHelper : IChatHelper
    {

        public void GetChatDefinition(ChatModel chat, Guid myId)
        {
            FetchChatName(chat, myId);
            FetchChatImage(chat, myId);
        }

        public void GetChatDefinition(IEnumerable<ChatModel> chats, Guid myId)
        {
            foreach (var chat in chats)
            {
                FetchChatName(chat, myId);
                FetchChatImage(chat, myId);
            }
            
        }

        private static void FetchChatImage(ChatModel chat, Guid myId)
        {
            if (string.IsNullOrEmpty(chat.Image))
            {
                chat.Image = chat.CreatorId == null
                    ? chat.Members.FirstOrDefault(m => m.Id != myId)?.Image
                    : null;
            }
        }
        private static void FetchChatName(ChatModel chat, Guid myId)
        {
            if (string.IsNullOrEmpty(chat.Name))
                chat.Name = string.Join(", ",chat.Members.Where(m => m.Id != myId).Select(m => m.Name));
        }
    }
}
