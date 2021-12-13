using BugPoint.Model.GeneralSetting;
using BugPoint.ViewModel.GeneralSettings;

namespace BugPoint.Data.Masters.Command
{
    public interface ISmtpSettingsCommand
    {
        int SaveSmtpSettings(SmtpEmailSettingsModel smtpEmailSettingsModel);
        int UpdateSmtpSettings(SmtpEmailSettingsModel smtpEmailSettingsModel);
        int SettingDefaultConnection(int? smtpProviderId);
    }
}