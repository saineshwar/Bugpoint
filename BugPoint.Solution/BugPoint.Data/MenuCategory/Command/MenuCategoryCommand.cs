using BugPoint.Data.EFContext;
using BugPoint.Model.MenuCategory;
using Microsoft.EntityFrameworkCore;


namespace BugPoint.Data.MenuCategory.Command
{
    public class MenuCategoryCommand : IMenuCategoryCommand
    {
        private readonly BugPointContext _bugPointContext;
        public MenuCategoryCommand(BugPointContext bugPointContext)
        {
            _bugPointContext = bugPointContext;
        }
        public int Add(MenuCategoryModel category)
        {
            _bugPointContext.MenuCategorys.Add(category);
            return _bugPointContext.SaveChanges();
        }

        public int Delete(MenuCategoryModel category)
        {
            _bugPointContext.Entry(category).State = EntityState.Deleted;
            return _bugPointContext.SaveChanges();
        }

        public int Update(MenuCategoryModel category)
        {
            _bugPointContext.Entry(category).State = EntityState.Modified;
            return _bugPointContext.SaveChanges();
        }
    }
}