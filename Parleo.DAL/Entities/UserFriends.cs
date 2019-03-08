using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Entities
{
    public class UserFriends
    {
        public Guid UserToId { get; set; }

        public virtual UserInfo UserTo { get; set; }

        public Guid UserFromId { get; set; }

        public virtual UserInfo UserFrom { get; set; }
        
        public int Status { get; set; }
    }
}
