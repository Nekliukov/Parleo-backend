using System;
using System.Collections.Generic;

namespace ParleoBackend
{
    public class UserInfoViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }        
        
        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }
        
        public decimal Latitude { get; set; }
        
        public decimal Longitude { get; set; }

        public string Mail { get; set; }

        /*public virtual ICollection<EventViewModel> Events { get; set; }

        public virtual ICollection<UserLanguageViewModel> Languages { get; set; }

        public virtual ICollection<UserFriendsViewModel> Friends { get; set; }   */     
    }
}
