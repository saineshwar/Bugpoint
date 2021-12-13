using BugPoint.Model.AssignedRoles;

namespace BugPoint.Data.UserMaster.Queries
{
    public interface IAssignedRolesQueries
    {
        AssignedRolesModel GetAssignedRolesDetailsbyUserId(long? userId);
    }
}