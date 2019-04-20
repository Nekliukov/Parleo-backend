using System;

namespace ParleoBackend.ViewModels.Entities
{
    public class UpdateUserViewModel
    {
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public UserLanguageViewModel[] Languages { get; set; }
    }
}