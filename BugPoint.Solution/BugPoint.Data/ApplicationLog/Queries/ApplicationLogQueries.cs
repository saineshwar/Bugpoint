using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using BugPoint.Data.EFContext;
using BugPoint.Model.ApplicationLog;
using BugPoint.ViewModel.MenuCategory;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.ApplicationLog.Queries
{
    public class ApplicationLogQueries : IApplicationLogQueries
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        public ApplicationLogQueries(BugPointContext bugPointContext, IConfiguration configuration)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
        }

        public IQueryable<NLogModel> ShowAllLogs(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from nLog in _bugPointContext.NLogModel

                                           select nLog
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryableMenuMaster = queryableMenuMaster.OrderByDescending(x => x.ID);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.Callsite.Contains(search) || m.Message.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public NLogModel ErrorDetails(int? id)
        {
            var result = (from nLog in _bugPointContext.NLogModel
                          where nLog.ID == id
                          select nLog).FirstOrDefault();

            return result;
        }

    }
}