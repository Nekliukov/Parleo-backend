using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Pages;
using System.Collections.Generic;

namespace Parleo.BLL.Models.Filters
{
    public class UserFilterModel : PageRequestModel
    {
        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public bool? Gender { get; set; }

        public int? MinDistance { get; set; }

        public int? MaxDistance { get; set; }

        public UserLanguageModel[] Languages { get; set; }
    }
}
