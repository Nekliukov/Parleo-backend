using System;
using System.Collections.Generic;

namespace ParleoBackend.ViewModels.Entities
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string AccountImage { get; set; }

        public string Name { get; set; }

        public string About { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public int DistanceFromCurrentUser { get; set; }

        public string Email { get; set; }

        public ICollection<MiniatureViewModel> CreatedEvents { get; set; }

        public ICollection<UserLanguageViewModel> Languages { get; set; }

        public ICollection<MiniatureViewModel> Friends { get; set; }

        public ICollection<MiniatureViewModel> AttendingEvents { get; set; }
    }
}
