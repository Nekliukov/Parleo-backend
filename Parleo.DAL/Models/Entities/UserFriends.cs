using System;

namespace Parleo.DAL.Models.Entities
{
    public class UserFriends
    {
        public Guid UserToId { get; set; }

        public User UserTo { get; set; }

        public Guid UserFromId { get; set; }

        public User UserFrom { get; set; }
        
        public int Status { get; set; }
    }
}
