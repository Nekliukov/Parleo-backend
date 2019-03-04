using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parleo.DAL.Entities
{
    public class UserAuth
    {
        [Key]
        [ForeignKey("UserInfo")]
        public Guid UserInfoId { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime LastLogin { get; set; }

        public UserInfo UserInfo { get; set; }
    }
}
