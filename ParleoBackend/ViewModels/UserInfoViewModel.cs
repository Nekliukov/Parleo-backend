using System;
using System.ComponentModel.DataAnnotations;

namespace ParleoBackend.ViewModels
{
    public class UserInfoViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        /*public virtual ICollection<EventViewModel> Events { get; set; }

        public virtual ICollection<UserLanguageViewModel> Languages { get; set; }

        public virtual ICollection<UserFriendsViewModel> Friends { get; set; }   */     
    }
}
