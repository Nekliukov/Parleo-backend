using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Entities
{
    public class UserFriends
    {
        public Guid UserToId { get; set; }
        public UserInfo UserTo { get; set; }

        public Guid UserFromId { get; set; }
        public UserInfo UserFrom { get; set; }

        // like outgoing or confirmed, need to brainstorm that
        public int Status { get; set; }
    }
}
