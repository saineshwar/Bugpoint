using System;
using System.Collections.Generic;
using System.Linq;
using BugPoint.Data.EFContext;
using BugPoint.ViewModel.GeneralSettings;
using System.Linq.Dynamic.Core;
using BugPoint.Common;
using BugPoint.Model.GeneralSetting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace BugPoint.Data.Masters.Queries
{
    public class SmtpSettingsQueries : ISmtpSettingsQueries
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IMemoryCache _cache;
        public SmtpSettingsQueries(BugPointContext bugPointContext, IMemoryCache cache)
        {
            _bugPointContext = bugPointContext;
            _cache = cache;
        }
        public SmtpEmailSettingsViewModel EditSmtpSettings(int? smtpProviderId)
        {

            try
            {
                var smtpEmailSettingsModel = (from smtp in _bugPointContext.SmtpEmailSettingsModel
                                              where smtp.SmtpProviderId == smtpProviderId
                                              select new SmtpEmailSettingsViewModel()
                                              {
                                                  SmtpProviderId = smtp.SmtpProviderId,
                                                  CreatedDate = smtp.CreatedOn,
                                                  Host = smtp.Host,
                                                  Name = smtp.Name,
                                                  Password = smtp.Password,
                                                  Port = smtp.Port,
                                                  SslProtocol = smtp.SslProtocol,
                                                  Timeout = smtp.Timeout,
                                                  TlSProtocol = smtp.TlSProtocol,
                                                  Username = smtp.Username,
                                                  EmailTo = smtp.EmailTo,
                                                  MailSender = smtp.MailSender
                                              }).FirstOrDefault();

                return smtpEmailSettingsModel;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public IQueryable<SmtpEmailSettingsGrid> ShowAllEmailSettings(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesRoleMasters = (from smtp in _bugPointContext.SmtpEmailSettingsModel
                                             select new SmtpEmailSettingsGrid()
                                             {
                                                 SmtpProviderId = smtp.SmtpProviderId,
                                                 CreatedDate = smtp.CreatedOn,
                                                 Host = smtp.Host,
                                                 Name = smtp.Name,
                                                 Password = smtp.Password,
                                                 Port = smtp.Port,
                                                 SslProtocol = smtp.SslProtocol,
                                                 Timeout = smtp.Timeout.ToString(),
                                                 TlSProtocol = smtp.TlSProtocol,
                                                 Username = smtp.Username,
                                                 IsDefault = smtp.IsDefault == true ? "Yes" : "No",
                                                 MailSender = smtp.MailSender,
                                                 EmailTo = smtp.EmailTo
                                             }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesRoleMasters = queryablesRoleMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryablesRoleMasters = queryablesRoleMasters.OrderByDescending(x => x.SmtpProviderId);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesRoleMasters = queryablesRoleMasters.Where(m => m.Name.Contains(search) || m.Name.Contains(search));
                }

                return queryablesRoleMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public SmtpEmailSettingsModel SmtpSettings(int? smtpProviderId)
        {

            try
            {
                var smtpEmailSettingsModel = (from smtp in _bugPointContext.SmtpEmailSettingsModel
                                              where smtp.SmtpProviderId == smtpProviderId
                                              select smtp).FirstOrDefault();

                return smtpEmailSettingsModel;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public SmtpEmailSettingsModel GetDefaultSmtpSettings()
        {

            try
            {
                #region Cache
                //SmtpEmailSettingsModel smtpEmailSettingsModel;
                //var key = $"{AllMemoryCacheKeys.PortalEmailKey}";
                //if (_cache.Get(key) == null)
                //{
                //    smtpEmailSettingsModel = (from smtp in _bugPointContext.SmtpEmailSettingsModel
                //                              where smtp.IsDefault == true
                //                              select smtp).FirstOrDefault();



                //    //MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                //    //{
                //    //    AbsoluteExpiration = DateTime.Now.AddMonths(1),
                //    //    Priority = CacheItemPriority.High
                //    //};

                //    //_cache.Set<SmtpEmailSettingsModel>(key, smtpEmailSettingsModel, cacheExpirationOptions);

                //}
                //else
                //{
                //    smtpEmailSettingsModel = _cache.Get(key) as SmtpEmailSettingsModel; 
                //} 
                #endregion

                var smtpEmailSettingsModel = (from smtp in _bugPointContext.SmtpEmailSettingsModel
                                              where smtp.IsDefault == true
                                              select smtp).FirstOrDefault();

                return smtpEmailSettingsModel;

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}