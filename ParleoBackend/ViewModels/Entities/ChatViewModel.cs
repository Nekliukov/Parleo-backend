using System;
using System.Collections.Generic;

namespace ParleoBackend.ViewModels.Entities
{
    public class ChatViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserMiniatureViewModel> Members { get; set; }

        public Guid? CreatorId { get; set; }

        public MessageViewModel LastMessage { get; set; }

        public int UnreadMessages { get; set; }
    }
}
