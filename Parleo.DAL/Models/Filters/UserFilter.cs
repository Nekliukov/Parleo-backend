using Parleo.DAL.Models.Pages;

namespace Parleo.DAL.Models.Filters
{
    public class UserFilter : PageRequest
    {
        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public bool? Gender { get; set; }

        public int? MaxDistance { get; set; }

        public FilteringLanguage[] Languages { get; set; }
    }
}
