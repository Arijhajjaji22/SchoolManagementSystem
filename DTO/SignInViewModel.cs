using System.ComponentModel.DataAnnotations;

namespace ProjetBrima.Models
{
    public class SignInViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
