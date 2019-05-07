using System;
using System.Collections.Generic;

namespace Parleo.BLL.Models.Entities
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string AccountImage { get; set; }

        public string About { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int DistanceFromCurrentUser { get; set; }

        public string Email { get; set; }

        public bool IsFriend { get; set; }

        public ICollection<UserLanguageModel> Languages { get; set; }

        public ICollection<HobbyModel> Hobbies { get; set; }
    }
}