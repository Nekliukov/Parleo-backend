using System;
using System.ComponentModel.DataAnnotations;

namespace ParleoBackend.ViewModels.Entities
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        /*public ICollection<EventViewModel> CreatedEvents { get; set; }

        public ICollection<UserLanguageViewModel> Languages { get; set; }

        public ICollection<UserFriendsViewModel> Friends { get; set; }   */     
    }
}
