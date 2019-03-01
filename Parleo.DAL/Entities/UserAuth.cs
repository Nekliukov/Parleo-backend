using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parleo.DAL.Entities
{
    class UserAuth
    {
        [ForeignKey("UserInfo")]
        public Guid UserInfoId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
