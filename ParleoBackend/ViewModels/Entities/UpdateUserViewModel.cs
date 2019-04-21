using System;

namespace ParleoBackend.ViewModels.Entities
{
    public class UpdateUserViewModel
    {
        public string Name { get; set; }

        public string About { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Email { get; set; }

        public UserLanguageViewModel[] Languages { get; set; }
    }
}