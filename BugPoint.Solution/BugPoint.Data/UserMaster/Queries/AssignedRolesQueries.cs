using System.Linq;
using BugPoint.Data.EFContext;
using BugPoint.Model.AssignedRoles;

namespace BugPoint.Data.UserMaster.Queries
{
    public class AssignedRolesQueries : IAssignedRolesQueries
    {
        private readonly BugPointContext _bugPointContext;
        public AssignedRolesQueries(BugPointContext oneFitnessVueContext)
        {
            _bugPointContext = oneFitnessVueContext;
        }
        public AssignedRolesModel GetAssignedRolesDetailsbyUserId(long? userId)
        {
            var assignedRoles = (from tempAssignedRole in _bugPointContext.AssignedRoles
                where tempAssignedRole.UserId == userId
                select tempAssignedRole).FirstOrDefault();
            return assignedRoles;
        }
    }
}