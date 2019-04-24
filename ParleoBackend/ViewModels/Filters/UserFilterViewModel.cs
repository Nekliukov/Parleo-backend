using ParleoBackend.ViewModels.Pages;

namespace ParleoBackend.ViewModels.Filters
{
    public class UserFilterViewModel : PageRequestViewModel
    {
        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public bool? Gender { get; set; }

        public int? MaxDistance { get; set; }

        public FilteringLanguageViewModel[] Languages { get; set; }
    }
}
