using System.ComponentModel.DataAnnotations;

namespace ParleoBackend.ViewModels.Entities
{
    public class UserRegistrationViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmedPassword { get; set; }
    }
}
