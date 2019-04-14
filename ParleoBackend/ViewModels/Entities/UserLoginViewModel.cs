using System.ComponentModel.DataAnnotations;

namespace ParleoBackend.ViewModels.Entities
{
    public class UserLoginViewModel
    {
        public string Password { get; set; }

        public string Email { get; set; }
    }
}
