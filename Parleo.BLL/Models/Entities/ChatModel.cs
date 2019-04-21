using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.BLL.Models.Entities
{
    public class ChatModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid CreatorId { get; set; }

        public MessageModel LastMessage { get; set; }

        public int UnreadMessages { get; set; }

    }
}
