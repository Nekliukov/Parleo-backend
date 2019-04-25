using Parleo.BLL.Models.Pages;

namespace Parleo.BLL.Models.Filters
{
    public class UserFilterModel : PageRequestModel
    {
        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public bool? Gender { get; set; }

        public int? MaxDistance { get; set; }

        public FilteringLanguageModel[] Languages { get; set; }
    }
}
