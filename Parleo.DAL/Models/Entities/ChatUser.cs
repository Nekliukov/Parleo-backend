using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Models.Entities
{
    public class ChatUser
    {
        public Guid ChatId { get; set; }

        public Chat Chat { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
