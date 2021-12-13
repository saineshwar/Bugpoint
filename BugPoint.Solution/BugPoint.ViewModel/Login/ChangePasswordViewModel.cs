using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.Login
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Old Password Required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password Required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        public string ConfirmPassword { get; set; }
    }
}