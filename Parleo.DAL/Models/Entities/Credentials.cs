using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parleo.DAL.Models.Entities
{
    public class Credentials
    {
        [Key]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTimeOffset LastLogin { get; set; }
        
        public User User { get; set; }
    }
}