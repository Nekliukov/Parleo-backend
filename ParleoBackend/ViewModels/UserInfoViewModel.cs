using System;
using System.Collections.Generic;

namespace ParleoBackend
{
    public class UserInfoViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }
        
        public decimal Latitude { get; set; }
        
        public decimal Longitude { get; set; }

        //Email?

        public ICollection<LanguageViewModel> Languages { get; set; }
    }
}
