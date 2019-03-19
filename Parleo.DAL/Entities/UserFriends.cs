using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Entities
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
