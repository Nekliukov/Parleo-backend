using System;
using System.ComponentModel.DataAnnotations;

namespace ParleoBackend.ViewModels.Entities
{
    public class AuthUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }
    }
}
