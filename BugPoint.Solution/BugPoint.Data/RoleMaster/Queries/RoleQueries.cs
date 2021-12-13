using System;
using System.Collections.Generic;
using System.Linq;
using BugPoint.Data.EFContext;
using BugPoint.Data.RoleMaster.Queries;
using BugPoint.Model.RoleMaster;
using BugPoint.ViewModel.RoleMaster;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Dynamic.Core;

namespace BugPoint.Data.RoleMaster.Queries
{
    public class RoleQueries : IRoleQueries
    {
        private readonly BugPointContext _bugPointContext;
        public RoleQueries(BugPointContext bugPointContext)
        {
            _bugPointContext = bugPointContext;
        }

        public bool CheckRoleNameExists(string roleName)
        {
            try
            {
                var result = (from role in _bugPointContext.RoleMasters
                              where role.RoleName == roleName
                              select role).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<RoleMasterGrid> ShowAllRoleMaster(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesRoleMasters = (from roleMaster in _bugPointContext.RoleMasters
                                             select new RoleMasterGrid()
                                             {
                                                 RoleId = roleMaster.RoleId,
                                                 RoleName = roleMaster.RoleName,
                                                 Status = roleMaster.Status == true ? "Active" : "InActive",
                                             }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesRoleMasters = queryablesRoleMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryablesRoleMasters = queryablesRoleMasters.OrderByDescending(x => x.RoleId);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesRoleMasters = queryablesRoleMasters.Where(m => m.RoleName.Contains(search) || m.RoleName.Contains(search));
                }

                return queryablesRoleMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public RoleMasterModel GetRoleMasterByroleId(int? roleId)
        {
            try
            {
                var rolesdata = _bugPointContext.RoleMasters.FirstOrDefault(s => s.RoleId == roleId);
                return rolesdata;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EditRoleMasterViewModel GetRoleMasterForEditByroleId(int? roleId)
        {
            try
            {
                var role = (from roles in _bugPointContext.RoleMasters
                            orderby roles.RoleName ascending
                            where roles.RoleId == roleId
                            select new EditRoleMasterViewModel()
                            {
                                RoleName = roles.RoleName,
                                Status = roles.Status,
                                RoleId = roles.RoleId
                            }).FirstOrDefault();
                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> ListofRoles()
        {
            var listofrolesdata = (from roles in _bugPointContext.RoleMasters
                                   orderby roles.RoleName ascending
                                   where roles.Status == true
                                   select new SelectListItem()
                                   {
                                       Text = roles.RoleName,
                                       Value = roles.RoleId.ToString()
                                   }).ToList();

            listofrolesdata.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });
            return listofrolesdata;
        }


        public List<SelectListItem> ListofDevandTesterLeadsRoles()
        {
            var temproles = new int[] { 4, 5, 6, 7,8 };

            var listofrolesdata = (from roles in _bugPointContext.RoleMasters
                                   orderby roles.RoleName ascending
                                   where roles.Status == true && temproles.Contains(roles.RoleId)
                                   select new SelectListItem()
                                   {
                                       Text = roles.RoleName,
                                       Value = roles.RoleId.ToString()
                                   }).ToList();

            listofrolesdata.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });
            return listofrolesdata;
        }


        public List<SelectListItem> GetAllActiveRoles()
        {
            try
            {
                var listofActiveMenu = (from roleMaster in _bugPointContext.RoleMasters
                                        orderby roleMaster.RoleName ascending
                                        where roleMaster.Status == true
                                        select new SelectListItem
                                        {
                                            Value = roleMaster.RoleId.ToString(),
                                            Text = roleMaster.RoleName
                                        }).ToList();

                listofActiveMenu.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}