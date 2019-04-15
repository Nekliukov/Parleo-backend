using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Parleo.BLL.Models.Entities
{
    public class UpdateUserModel
    {
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public ICollection<UserLanguageModel> Languages { get; set; }
    }
}