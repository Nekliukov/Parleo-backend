using ParleoBackend.ViewModels;
using System.Collections.Generic;

namespace ParleoBackend.ViewModels
{
    public class UserFilterViewModel : PageRequestViewModel
    {
        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        public bool Gender { get; set; }

        public int MinDistance { get; set; }

        public int MaxDistance { get; set; }

        public List<LanguageViewModel> Languages { get; set; }
    }
}
