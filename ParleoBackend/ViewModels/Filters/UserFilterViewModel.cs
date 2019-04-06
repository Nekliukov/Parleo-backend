using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Pages;
using System.Collections.Generic;

namespace ParleoBackend.ViewModels.Filters
{
    public class UserFilterViewModel : PageRequestViewModel
    {
        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public bool? Gender { get; set; }

        public int? MinDistance { get; set; }

        public int? MaxDistance { get; set; }

        public List<UserLanguageViewModel> Languages { get; set; }
    }
}
