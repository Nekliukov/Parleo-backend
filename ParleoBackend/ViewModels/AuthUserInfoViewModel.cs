using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParleoBackend.ViewModels
{
    public class AuthUserInfoViewModel
    {
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
