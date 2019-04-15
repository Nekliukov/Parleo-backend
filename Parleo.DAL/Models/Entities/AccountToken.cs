using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parleo.DAL.Models.Entities
{
    public class AccountToken
    {
        [Key]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public User User { get; set; }
    }
}
