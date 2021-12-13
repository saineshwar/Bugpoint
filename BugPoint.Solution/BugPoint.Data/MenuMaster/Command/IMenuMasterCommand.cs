using BugPoint.Model.MenuMaster;

namespace BugPoint.Data.MenuMaster.Command
{
    public interface IMenuMasterCommand
    {
        int Add(MenuMasterModel menuMaster);
        int Delete(MenuMasterModel menuMaster);
        int Update(MenuMasterModel menuMaster);
    }
}