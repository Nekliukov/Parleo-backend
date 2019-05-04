using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParleoBackend.ViewModels.Entities
{
    public class CreateGroupChatViewModel
    {
        public string Name { get; set; }

        public Guid[] Members { get; set; }

        public Guid? EventId { get; set; }
    }
}
