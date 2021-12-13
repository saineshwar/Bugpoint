using System.Linq;
using BugPoint.Model.GeneralSetting;
using BugPoint.ViewModel.GeneralSettings;

namespace BugPoint.Data.Masters.Queries
{
    public interface ISmtpSettingsQueries
    {
        SmtpEmailSettingsViewModel EditSmtpSettings(int? smtpProviderId);

        IQueryable<SmtpEmailSettingsGrid> ShowAllEmailSettings(string sortColumn, string sortColumnDir,
            string search);

        SmtpEmailSettingsModel SmtpSettings(int? smtpProviderId);

        SmtpEmailSettingsModel GetDefaultSmtpSettings();
    }
}