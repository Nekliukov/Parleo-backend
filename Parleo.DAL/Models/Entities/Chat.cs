using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Models.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public User Creator { get; set; }

        public ICollection<ChatUser> Members { get; set; }

        public ICollection<Message> Messages { get; set; }

        public Event Event { get; set; }
    }
}
