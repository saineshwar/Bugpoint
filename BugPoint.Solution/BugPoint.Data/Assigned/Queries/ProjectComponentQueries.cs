using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using BugPoint.Data.EFContext;
using BugPoint.Model.Assigned;
using BugPoint.ViewModel.Assigned;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace BugPoint.Data.Assigned.Queries
{
    public class ProjectComponentQueries : IProjectComponentQueries
    {
        private readonly BugPointContext _bugPointContext;
        public ProjectComponentQueries(BugPointContext bugPointContext)
        {
            _bugPointContext = bugPointContext;
        }

        public bool CheckProjectComponentExists(int projectId, string componentName)
        {
            var result = (from projectsModel in _bugPointContext.ProjectComponentModel.AsNoTracking()
                          where projectsModel.ProjectId == projectId && projectsModel.ComponentName == componentName
                          select projectsModel).Any();

            return result;

        }

        public List<SelectListItem> GetProjectComponentsList(int? projectId)
        {
            var listofdata = (from projectComponent in _bugPointContext.ProjectComponentModel
                              join users in _bugPointContext.UserMasters on projectComponent.AssignedTo equals users.UserId
                              orderby projectComponent.ComponentName ascending
                              where projectComponent.ProjectId == projectId && projectComponent.Status == true
                              select new SelectListItem
                              {
                                  Text = $"{projectComponent.ComponentName} | {users.FirstName} {users.LastName}",
                                  Value = projectComponent.ProjectComponentId.ToString()

                              }).ToList();


            listofdata.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return listofdata;

        }


        public List<SelectListItem> GetProjectComponentsListByUserId(int? projectId, int? userId)
        {
            var listofdata = (from projectComponent in _bugPointContext.ProjectComponentModel
                              join users in _bugPointContext.UserMasters on projectComponent.AssignedTo equals users.UserId
                              orderby projectComponent.ComponentName ascending
                              where projectComponent.ProjectId == projectId && projectComponent.Status == true && projectComponent.AssignedTo == userId
                              select new SelectListItem
                              {
                                  Text = $"{projectComponent.ComponentName} | {users.FirstName} {users.LastName}",
                                  Value = projectComponent.ProjectComponentId.ToString()

                              }).ToList();


            listofdata.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return listofdata;

        }

        public ProjectComponentModel ProjectComponentDetailsByProjectId(long projectComponentId)
        {
            var result = (from projectsModel in _bugPointContext.ProjectComponentModel.AsNoTracking()
                          where projectsModel.ProjectComponentId == projectComponentId
                          orderby projectsModel.ComponentName ascending
                          select projectsModel).FirstOrDefault();

            return result;
        }
        public EditProjectComponentViewModel GetProjectComponentDetailsByProjectId(int projectComponentId)
        {
            var result = (from projectsModel in _bugPointContext.ProjectComponentModel.AsNoTracking()
                          where projectsModel.ProjectComponentId == projectComponentId
                          select new EditProjectComponentViewModel()
                          {
                              AssignBugto = projectsModel.AssignedTo,
                              ProjectComponentId = projectsModel.ProjectComponentId,
                              ComponentDescription = projectsModel.ComponentDescription,
                              ComponentName = projectsModel.ComponentName,
                              ProjectId = projectsModel.ProjectId,
                              Status = projectsModel.Status
                          }).FirstOrDefault();

            return result;
        }

        //

        public IQueryable<ProjectComponentGrid> ShowAllProjectComponents(string sortColumn, string sortColumnDir, string search, int projectid)
        {
            try
            {
                var queryables = (from projectComponent in _bugPointContext.ProjectComponentModel.AsNoTracking()
                                  join users in _bugPointContext.UserMasters on projectComponent.AssignedTo equals users.UserId
                                  where projectComponent.ProjectId == projectid
                                  select new ProjectComponentGrid()
                                  {
                                      ProjectComponentId = projectComponent.ProjectComponentId,
                                      ComponentName = projectComponent.ComponentName,
                                      Status = projectComponent.Status == true ? "Active" : "InActive",
                                      CreatedOn = projectComponent.CreatedOn,
                                      ComponentDescription = projectComponent.ComponentDescription,
                                      Username = users.UserName
                                  }).AsQueryable();


                queryables = queryables.OrderByDescending(x => x.ProjectComponentId);


                if (!string.IsNullOrEmpty(search))
                {
                    queryables = queryables.Where(m => m.ComponentName.Contains(search) || m.ComponentName.Contains(search));
                }

                return queryables;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}