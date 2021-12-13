using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using BugPoint.Common;
using BugPoint.Data.EFContext;
using BugPoint.Model.MenuMaster;
using BugPoint.ViewModel.MenuMaster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BugPoint.Data.MenuMaster.Queries
{
    public class MenuMasterQueries : IMenuMasterQueries
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IMemoryCache _cache;
        public MenuMasterQueries(BugPointContext bugPointContext, IMemoryCache cache)
        {
            _bugPointContext = bugPointContext;
            _cache = cache;
        }

        public bool CheckMenuNameExists(string menuName)
        {
            try
            {
                var result = (from menu in _bugPointContext.MenuMasters
                              where menu.MenuName == menuName
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckMenuExists(string menuName, int? roleId, int? categoryId)
        {
            try
            {
                var result = (from menu in _bugPointContext.MenuMasters.AsNoTracking()
                              where menu.MenuName == menuName && menu.RoleId == roleId && menu.MenuCategoryId == categoryId
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<MenuMasterGrid> ShowAllMenus(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from menu in _bugPointContext.MenuMasters
                                           join category in _bugPointContext.MenuCategorys on menu.MenuCategoryId equals category.MenuCategoryId
                                           join roleMaster in _bugPointContext.RoleMasters on menu.RoleId equals roleMaster.RoleId
                                           orderby menu.MenuId descending
                                           select new MenuMasterGrid()
                                           {
                                               Status = menu.Status == true ? "Active" : "InActive",
                                               ActionMethod = menu.ActionMethod,
                                               MenuName = menu.MenuName,
                                               ControllerName = menu.ControllerName,
                                               MenuId = menu.MenuId,
                                               RoleName = roleMaster.RoleName,
                                               MenuCategoryName = category.MenuCategoryName,
                                               Area = string.IsNullOrEmpty(menu.Area) ? "-" : menu.Area
                                           }
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryableMenuMaster = queryableMenuMaster.OrderByDescending(x=>x.MenuId);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.MenuName.Contains(search) || m.MenuName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public EditMenuMasterViewModel GetMenuByMenuId(int? menuId)
        {
            try
            {
                var editmenu = (from menu in _bugPointContext.MenuMasters
                                where menu.MenuId == menuId
                                select new EditMenuMasterViewModel()
                                {
                                    Status = menu.Status,
                                    ActionMethod = menu.ActionMethod,
                                    MenuName = menu.MenuName,
                                    ControllerName = menu.ControllerName,
                                    MenuId = menu.MenuId,
                                    RoleId = menu.RoleId,
                                    MenuCategoryId = menu.MenuCategoryId,
                                    Area = menu.Area
                                }).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MenuMasterModel GetMenuMasterByMenuId(int? menuId)
        {
            try
            {
                var editmenu = (from menu in _bugPointContext.MenuMasters
                                where menu.MenuId == menuId
                                select menu).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool EditValidationCheck(int? menuId, EditMenuMasterViewModel editMenu)
        {
            var result = (from menu in _bugPointContext.MenuMasters.AsNoTracking()
                          where menu.MenuId == menuId
                          select menu).SingleOrDefault();

            if (result != null && (editMenu.MenuCategoryId == result.MenuCategoryId && editMenu.RoleId == result.RoleId && editMenu.MenuName == result.MenuName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SelectListItem> ListofMenusbyRoleId(RequestMenus requestMenus)
        {
            var listofactiveMenus = (from tempmenu in _bugPointContext.MenuMasters
                                     where tempmenu.Status == true && tempmenu.RoleId == requestMenus.RoleId && tempmenu.MenuCategoryId == requestMenus.CategoryId
                                     orderby tempmenu.MenuId ascending
                                     select new SelectListItem
                                     {
                                         Value = tempmenu.MenuId.ToString(),
                                         Text = tempmenu.MenuName
                                     }).ToList();

            listofactiveMenus.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });
            return listofactiveMenus;
        }

        public List<MenuMasterModel> GetMenuByRoleId(int? roleId, int? menuCategoryId)
        {
            var key = $"{AllMemoryCacheKeys.MenuMasterKey}_{roleId}";
            List<MenuMasterModel> menuList;
            if (_cache.Get(key) == null)
            {
                var result = (from menu in _bugPointContext.MenuMasters.AsNoTracking()
                              orderby menu.SortingOrder ascending
                              where menu.Status == true
                              select menu).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7),
                    Priority = CacheItemPriority.Normal
                };

                _cache.Set<List<MenuMasterModel>>(key, result, cacheExpirationOptions);

                menuList = ((from menu in result
                             orderby menu.SortingOrder ascending
                             where menu.Status == true && menu.RoleId == roleId && menu.MenuCategoryId == menuCategoryId
                             select menu).ToList());
            }
            else
            {
                var storeMenuMasters = _cache.Get(key) as List<MenuMasterModel>;

                menuList = ((from menu in storeMenuMasters
                             orderby menu.SortingOrder ascending
                             where menu.Status == true && menu.RoleId == roleId && menu.MenuCategoryId == menuCategoryId
                             select menu).ToList());
            }

            return menuList;
        }

        public List<MenuMasterOrderingVm> GetListofMenu(int roleId, int menuCategoryId)
        {
            var listofactiveMenus = (from tempmenu in _bugPointContext.MenuMasters
                                     where tempmenu.Status == true && tempmenu.RoleId == roleId && tempmenu.MenuCategoryId == menuCategoryId
                                     orderby tempmenu.SortingOrder ascending
                                     select new MenuMasterOrderingVm
                                     {
                                         MenuId = tempmenu.MenuId,
                                         MenuName = tempmenu.MenuName
                                     }).ToList();

            return listofactiveMenus;
        }

        public List<SelectListItem> ListofMenubyRoleIdSelectListItem(int roleId, int menuCategoryId)
        {
            var listofactiveMenus = (from tempmenu in _bugPointContext.MenuMasters
                                     where tempmenu.Status == true && tempmenu.RoleId == roleId && tempmenu.MenuCategoryId == menuCategoryId
                                     orderby tempmenu.SortingOrder ascending
                                     select new SelectListItem
                                     {
                                         Value = tempmenu.MenuId.ToString(),
                                         Text = tempmenu.MenuName
                                     }).ToList();

            listofactiveMenus.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "---Select Main Menu---"
            });

            return listofactiveMenus;
        }
    }
}