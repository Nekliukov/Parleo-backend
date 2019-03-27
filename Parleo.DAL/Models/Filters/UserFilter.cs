using Parleo.DAL.Models.Entities;
using Parleo.DAL.Models.Pages;
using System.Collections.Generic;

namespace Parleo.DAL.Models.Filters
{
    public class UserFilter : PageRequest
    {
        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public bool? Gender { get; set; }

        public int? MinDistance { get; set; }

        public int? MaxDistance { get; set; }

        public List<UserLanguage> Languages { get; set; }
    }
}
