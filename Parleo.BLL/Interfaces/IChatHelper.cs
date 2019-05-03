using System;
using System.Collections.Generic;
using System.Text;
using Parleo.BLL.Models.Entities;

namespace Parleo.BLL.Interfaces
{
    public interface IChatHelper
    {
        void GetChatDefinition(ChatModel chat, Guid myId);

        void GetChatDefinition(IEnumerable<ChatModel> chats, Guid myId);
    }
}
