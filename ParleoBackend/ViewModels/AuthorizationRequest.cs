using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParleoBackend.ViewModels
{
    public class AuthorizationRequest
    {
        public string Password { get; set; }

        public string Email { get; set; }
    }
}
