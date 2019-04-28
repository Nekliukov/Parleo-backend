using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParleoBackend.ViewModels.Entities
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Status { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? ViewedOn { get; set; }

        public bool IsDeleted { get; set; }

        public Guid SenderId { get; set; }

        public Guid ChatId { get; set; }
    }
}
