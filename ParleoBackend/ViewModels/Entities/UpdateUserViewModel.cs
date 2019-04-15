using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParleoBackend.ViewModels.Entities
{
    public class UpdateUserViewModel
    {
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public UserLanguageViewModel[] Languages { get; set; }
    }
}