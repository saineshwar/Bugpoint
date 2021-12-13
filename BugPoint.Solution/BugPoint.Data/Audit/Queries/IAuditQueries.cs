using System.Collections.Generic;
using BugPoint.ViewModel.Audit;

namespace BugPoint.Data.Audit.Queries
{
    public interface IAuditQueries
    {
        List<AuditViewModel> GetUserActivity(long? userId);
    }
}