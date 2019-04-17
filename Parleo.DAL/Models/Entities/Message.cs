using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Parleo.DAL.Models.Entities
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Text { get; set; }

        public string Status { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ViewedOn { get; set; }

        public bool IsDeleted { get; set; }

        public User Sender { get; set; }

        public Chat Chat { get; set; }

        public Guid ChatId { get; set; }
    }
}
