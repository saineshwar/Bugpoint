using BugPoint.ViewModel.Login;

namespace BugPoint.Data.UserMaster.Command
{
    public interface IVerificationCommand
    {
        string InsertResetPasswordVerificationToken(ResetPasswordVerification resetPassword);
        string UpdatePasswordandVerificationStatus(UpdateResetPasswordVerification resetPassword);
    }
}