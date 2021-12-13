using BugPoint.Data.EFContext;
using BugPoint.Model.MenuMaster;
using Microsoft.EntityFrameworkCore;

namespace BugPoint.Data.MenuMaster.Command
{
    public class MenuMasterCommand : IMenuMasterCommand
    {
        private readonly BugPointContext _bugPointContext;
        public MenuMasterCommand(BugPointContext bugPointContext)
        {
            _bugPointContext = bugPointContext;
        }

        public int Add(MenuMasterModel menuMaster)
        {
            _bugPointContext.MenuMasters.Add(menuMaster);
            return _bugPointContext.SaveChanges();
        }

        public int Delete(MenuMasterModel menuMaster)
        {
            _bugPointContext.Entry(menuMaster).State = EntityState.Deleted;
            return _bugPointContext.SaveChanges();
        }

        public int Update(MenuMasterModel menuMaster)
        {
            _bugPointContext.Entry(menuMaster).State = EntityState.Modified;
            return _bugPointContext.SaveChanges();
        }
    }
}