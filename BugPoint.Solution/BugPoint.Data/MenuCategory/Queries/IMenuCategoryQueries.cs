using System.Collections.Generic;
using System.Linq;
using BugPoint.Model.MenuCategory;
using BugPoint.ViewModel.MenuCategory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.Data.MenuCategory.Queries
{
    public interface IMenuCategoryQueries
    {
        MenuCategoryModel GetCategoryByMenuCategoryId(int? menuCategoryId);
        EditMenuCategoriesViewModel GetCategoryByMenuCategoryIdForEdit(int? menuCategoryId);
        List<SelectListItem> GetCategorybyRoleId(int? roleId);
        int GetCategoryCount(string menuCategoryName);
        IQueryable<MenuCategoryGridViewModel> ShowAllMenusCategory(string sortColumn, string sortColumnDir,
            string search);
        bool CheckCategoryNameExists(string menuCategoryName, int roleId);
        List<MenuCategoryModel> GetCategoryByRoleId(int? roleId);
        List<MenuCategoryOrderingVm> ListofMenubyRoleCategoryId(int roleId);
    }
}