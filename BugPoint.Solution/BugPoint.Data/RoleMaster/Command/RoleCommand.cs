using BugPoint.Data.EFContext;
using BugPoint.Model.RoleMaster;
using Microsoft.EntityFrameworkCore;

namespace BugPoint.Data.RoleMaster.Command
{
    public class RoleCommand : IRoleCommand
    {
        private readonly BugPointContext _bugPointContext;
        public RoleCommand(BugPointContext bugPointContext)
        {
            _bugPointContext = bugPointContext;
        }

        public int Delete(RoleMasterModel roleMaster)
        {
            _bugPointContext.Entry(roleMaster).State = EntityState.Deleted;
            return _bugPointContext.SaveChanges();
        }

        public int Add(RoleMasterModel roleMaster)
        {
            _bugPointContext.RoleMasters.Add(roleMaster);
            return _bugPointContext.SaveChanges();
        }

        public int Update(RoleMasterModel roleMaster)
        {
            _bugPointContext.Entry(roleMaster).State = EntityState.Modified;
            return _bugPointContext.SaveChanges();
        }
    }
}