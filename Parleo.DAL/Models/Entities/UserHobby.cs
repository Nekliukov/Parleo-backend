using System;

namespace Parleo.DAL.Models.Entities
{
    public class UserHobby
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public string HobbyName { get; set; }

        public Hobby Hobby { get; set; }
    }
}
