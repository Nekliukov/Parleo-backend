using System;

namespace Parleo.BLL.Models.Entities
{
    public class UpdateUserModel
    {
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Email { get; set; }

        public UserLanguageModel[] Languages { get; set; }
    }
}