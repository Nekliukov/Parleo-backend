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
            FindChatName(chat, myId);
            FindChatImage(chat, myId);
        }

        public void GetChatDefinition(IEnumerable<ChatModel> chats, Guid myId)
        {
            foreach (var chat in chats)
            {
                FindChatName(chat, myId);
                FindChatImage(chat, myId);
            }
            
        }

        private static void FindChatImage(ChatModel chat, Guid myId)
        {
            if (string.IsNullOrEmpty(chat.Image))
            {
                if (chat.CreatorId == null)
                    chat.Image = chat.Members.FirstOrDefault(m => m.Id != myId)?.Image;
            }
        }
        private static void FindChatName(ChatModel chat, Guid myId)
        {
            if (string.IsNullOrEmpty(chat.Name))
            {
                if (chat.CreatorId == null)
                    chat.Name = chat.Members.FirstOrDefault(m => m.Id != myId)?.Name;
            }
        }
    }
}
