using System.ComponentModel.DataAnnotations;

namespace ParleoBackend.ViewModels.Entities
{
    public class AuthorizationViewModel
    {
        [Required]
        [MinLength(8, ErrorMessage = "min length 8 symbols")] //move to settings
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
