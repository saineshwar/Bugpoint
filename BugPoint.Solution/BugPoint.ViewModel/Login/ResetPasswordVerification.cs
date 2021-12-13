using System;

namespace BugPoint.ViewModel.Login
{
    public class ResetPasswordVerification
    {
        public string GeneratedToken { get; set; }
        public DateTime? GeneratedDate { get; set; }
        public long UserId { get; set; }
    }

    public class UpdateResetPasswordVerification
    {
        public string GeneratedToken { get; set; }
        public string Password { get; set; }
        public int? UserId { get; set; }
    }
}