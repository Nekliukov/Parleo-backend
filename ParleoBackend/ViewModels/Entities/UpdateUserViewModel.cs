using System;

namespace ParleoBackend.ViewModels.Entities
{
    public class UpdateUserViewModel
    {
        public string Name { get; set; }

        public string About { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public UserLanguageViewModel[] Languages { get; set; }

        public string[] Hobbies { get; set; }
    }
}