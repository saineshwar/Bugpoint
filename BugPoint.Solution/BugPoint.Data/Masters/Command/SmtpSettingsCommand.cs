using System;
using System.Data;
using System.Linq;
using BugPoint.Data.EFContext;
using BugPoint.Model.GeneralSetting;
using BugPoint.ViewModel.GeneralSettings;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace BugPoint.Data.Masters.Command
{
    public class SmtpSettingsCommand : ISmtpSettingsCommand
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        public SmtpSettingsCommand(BugPointContext bugPointContext, IConfiguration configuration)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
        }

        public int SaveSmtpSettings(SmtpEmailSettingsModel smtpEmailSettingsModel)
        {
            try
            {

                _bugPointContext.SmtpEmailSettingsModel.Add(smtpEmailSettingsModel);
                return _bugPointContext.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateSmtpSettings(SmtpEmailSettingsModel smtpEmailSettingsModel)
        {
            try
            {
                _bugPointContext.Entry(smtpEmailSettingsModel).State = EntityState.Modified;
                return _bugPointContext.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int SettingDefaultConnection(int? smtpProviderId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@SmtpProviderId", smtpProviderId);
                var result = con.Execute("Usp_SMTPEmailSettings_SetDefault", param, null, 0, CommandType.StoredProcedure);
                if (result > 0)
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}