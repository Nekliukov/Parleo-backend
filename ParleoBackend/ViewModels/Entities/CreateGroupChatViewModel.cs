using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParleoBackend.ViewModels.Entities
{
    public class CreateGroupChatViewModel
    {
        public string Name { get; set; }

        //public string Image { get; set; }

        public Guid[] Members { get; set; }
    }
}
