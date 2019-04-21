using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.BLL.Models.Entities
{
    public class MessageModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Status { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ViewedOn { get; set; }

        public bool IsDeleted { get; set; }

        public Guid SenderId { get; set; }

        public Guid ChatId { get; set; }
    }
}
