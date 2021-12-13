using BugPoint.Model.RoleMaster;

namespace BugPoint.Data.RoleMaster.Command
{
    public interface IRoleCommand
    {
        int Delete(RoleMasterModel roleMaster);
        int Add(RoleMasterModel roleMaster);
        int Update(RoleMasterModel roleMaster);
    }
}