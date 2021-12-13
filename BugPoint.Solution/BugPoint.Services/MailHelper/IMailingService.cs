using System.Threading.Tasks;

namespace BugPoint.Services.MailHelper
{
    public interface IMailingService
    {
        bool SendEmailAsync(SendingMailRequest mailRequest);
    }
}