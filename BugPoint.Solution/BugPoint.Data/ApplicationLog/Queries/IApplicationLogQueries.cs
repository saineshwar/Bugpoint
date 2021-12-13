using System.Linq;
using BugPoint.Model.ApplicationLog;

namespace BugPoint.Data.ApplicationLog.Queries
{
    public interface IApplicationLogQueries
    {
        IQueryable<NLogModel> ShowAllLogs(string sortColumn, string sortColumnDir, string search);
        NLogModel ErrorDetails(int? id);
    }
}