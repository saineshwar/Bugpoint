using System.Linq;
using BugPoint.ViewModel.MovingBugs;

namespace BugPoint.Data.Assigned.Queries
{
    public interface IMovingBugsQueries
    {
        IQueryable<MovingBugsGrid> ShowAllAssignedBugs(string sortColumn, string sortColumnDir, string search,
            int? projectid, int userId, int roleId);
    }
}