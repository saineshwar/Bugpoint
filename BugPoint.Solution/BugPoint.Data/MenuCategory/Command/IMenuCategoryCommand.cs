using BugPoint.Model.MenuCategory;

namespace BugPoint.Data.MenuCategory.Command
{
    public interface IMenuCategoryCommand
    {
        int Add(MenuCategoryModel category);
        int Update(MenuCategoryModel category);
        int Delete(MenuCategoryModel category);
    }
}