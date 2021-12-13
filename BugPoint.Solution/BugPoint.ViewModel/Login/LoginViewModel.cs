using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.ViewModel.Login
{
    [NotMapped]
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username Required")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Invalid username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [MaxLength(256)]
        public string Password { get; set; }

        public string Hdrandomtoken { get; set; }
    }
}