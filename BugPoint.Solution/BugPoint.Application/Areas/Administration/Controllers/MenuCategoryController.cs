using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.MenuCategory.Command;
using BugPoint.Data.MenuCategory.Queries;
using BugPoint.Data.RoleMaster.Queries;
using BugPoint.Model.MenuCategory;
using BugPoint.ViewModel.MenuCategory;
using Microsoft.AspNetCore.Http;

namespace BugPoint.Application.Areas.Administration.Controllers
{
    [Area("Administration")]
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class MenuCategoryController : Controller
    {
        private readonly IRoleQueries _roleQueries;
        private readonly IMenuCategoryQueries _menuCategoryQueries;
        private readonly IMenuCategoryCommand _menuCategoryCommand;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        public MenuCategoryController(
            IRoleQueries roleQueries,
            IMenuCategoryQueries menuCategoryQueries,
            IMenuCategoryCommand menuCategoryCommand, INotificationService notificationService, IMapper mapper)
        {
            _roleQueries = roleQueries;
            _menuCategoryQueries = menuCategoryQueries;
            _menuCategoryCommand = menuCategoryCommand;
            _notificationService = notificationService;
            _mapper = mapper;
        }
        public IActionResult Create()
        {
            var addCategoriesVm = new CreateMenuCategoryViewModel()
            {
                ListofRoles = _roleQueries.ListofRoles(),
                Status = true
            };
            return View(addCategoriesVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateMenuCategoryViewModel menuCategory)
        {
            menuCategory.ListofRoles = _roleQueries.ListofRoles();

            if (ModelState.IsValid)
            {
                if (_menuCategoryQueries.CheckCategoryNameExists(menuCategory.MenuCategoryName, menuCategory.RoleId))
                {
                    ModelState.AddModelError("", "Menu Category Already Exists!");
                    return View(menuCategory);
                }
                else
                {
                    var mappedobject = _mapper.Map<MenuCategoryModel>(menuCategory);
                    mappedobject.CreatedOn = DateTime.Now;
                    mappedobject.CreatedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                    var result = _menuCategoryCommand.Add(mappedobject);
                    if (result > 0)
                    {
                        _notificationService.SuccessNotification("Message",
                            "The Menu Category was added Successfully!");
                        return RedirectToAction("Index", "MenuCategory");
                    }
                }
            }

            return View(menuCategory);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllMenuCategory()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var rolesdata = _menuCategoryQueries.ShowAllMenusCategory(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = rolesdata.Count();
                var data = rolesdata.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var editdata = _menuCategoryQueries.GetCategoryByMenuCategoryIdForEdit(id);
                editdata.ListofRoles = _roleQueries.ListofRoles();
                return View(editdata);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditMenuCategoriesViewModel category)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var editdata = _menuCategoryQueries.GetCategoryByMenuCategoryId(category.MenuCategoryId);

                    if (editdata.RoleId == category.RoleId && editdata.MenuCategoryName == category.MenuCategoryName)
                    {
                        var categories = new MenuCategoryModel()
                        {
                            RoleId = category.RoleId,
                            MenuCategoryName = category.MenuCategoryName,
                            Status = category.Status,
                            MenuCategoryId = category.MenuCategoryId,
                            ModifiedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId),
                            ModifiedOn = DateTime.Now,
                        };

                        _menuCategoryCommand.Update(categories);
                        TempData["CategoryUpdateMessages"] = CommonMessages.CategorySuccessMessages;
                    }
                    else if (_menuCategoryQueries.CheckCategoryNameExists(category.MenuCategoryName, category.RoleId))
                    {
                        ModelState.AddModelError("", CommonMessages.CategoryAlreadyExistsMessages);
                        category.ListofRoles = _roleQueries.ListofRoles();
                        return View(category);
                    }
                    else
                    {
                        var categories = new MenuCategoryModel()
                        {
                            RoleId = category.RoleId,
                            MenuCategoryName = category.MenuCategoryName,
                            Status = category.Status,
                            MenuCategoryId = category.MenuCategoryId,
                            ModifiedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId),
                            ModifiedOn = DateTime.Now,
                        };

                        _menuCategoryCommand.Update(categories);
                        TempData["CategoryUpdateMessages"] = CommonMessages.CategorySuccessMessages;
                    }
                }

                category.ListofRoles = _roleQueries.ListofRoles();
                return View(category);
            }
            catch
            {
                throw;
            }
        }
    }
}
