using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using BugPoint.Data.EFContext;
using BugPoint.Model.Project;
using BugPoint.ViewModel.Project;
using BugPoint.ViewModel.RoleMaster;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.Data.Project.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly BugPointContext _bugPointContext;
        public ProjectQueries(BugPointContext bugPointContext)
        {
            _bugPointContext = bugPointContext;
        }

        public bool CheckProjectNameExists(string projectname)
        {
            var result = (from projectsModel in _bugPointContext.ProjectsModel
                          where projectsModel.ProjectName == projectname
                          select projectsModel).Any();

            return result;

        }

        public IQueryable<ProjectGrid> ShowAllProjects(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesprojects = (from projectsModel in _bugPointContext.ProjectsModel
                                          select new ProjectGrid()
                                          {
                                              ProjectId = projectsModel.ProjectId,
                                              ProjectName = projectsModel.ProjectName,
                                              Status = projectsModel.Status == true ? "Active" : "InActive",
                                              ProjectDescription = projectsModel.ProjectDescription,
                                              CreatedOn = projectsModel.CreatedOn
                                          }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesprojects = queryablesprojects.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryablesprojects = queryablesprojects.OrderByDescending(x => x.ProjectId);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    queryablesprojects = queryablesprojects.Where(m => m.ProjectName.Contains(search) || m.ProjectName.Contains(search));
                }

                return queryablesprojects;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public EditProjectViewModel EditProject(int? projectId)
        {
            try
            {
                var role = (from edit in _bugPointContext.ProjectsModel
                            where edit.ProjectId == projectId
                            select new EditProjectViewModel()
                            {
                                ProjectDescription = edit.ProjectDescription,
                                Status = edit.Status,
                                ProjectId = edit.ProjectId,
                                ProjectName = edit.ProjectName
                            }).FirstOrDefault();
                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProjectsModel GetProjectDetails(int? projectId)
        {
            try
            {
                var role = (from edit in _bugPointContext.ProjectsModel
                            where edit.ProjectId == projectId
                            select edit).FirstOrDefault();
                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetProjectbyProjectName(string projectname)
        {
            var result = (from projectsModel in _bugPointContext.ProjectsModel
                orderby projectsModel.ProjectName ascending
                          where projectsModel.ProjectName.Contains(projectname) && projectsModel.Status == true
                          select new SelectListItem
                          {
                              Text = projectsModel.ProjectName,
                              Value = projectsModel.ProjectId.ToString()

                          }).ToList();

            return result;

        }

        public List<SelectListItem> GetProjectList()
        {
            var listofdata = (from projectsModel in _bugPointContext.ProjectsModel
                orderby  projectsModel.ProjectName ascending 
                              where projectsModel.Status == true
                              
                              select new SelectListItem
                              {
                                  Text = projectsModel.ProjectName,
                                  Value = projectsModel.ProjectId.ToString()

                              }).ToList();


            return listofdata;

        }


        public List<SelectListItem> GetAllProjectList()
        {
            var listofdata = (from projectsModel in _bugPointContext.ProjectsModel
                orderby projectsModel.ProjectName ascending
                              where projectsModel.Status == true

                              select new SelectListItem
                              {
                                  Text = projectsModel.ProjectName,
                                  Value = projectsModel.ProjectId.ToString()

                              }).ToList();

            listofdata.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return listofdata;

        }


        public List<SelectListItem> GetAssignedProjectList(int? userId)
        {
            var listofdata = (from assignedProject in _bugPointContext.AssignedProjectModel
                              where assignedProject.UserId == userId
                              join projectsModel in _bugPointContext.ProjectsModel on assignedProject.ProjectId equals projectsModel.ProjectId
                              orderby projectsModel.ProjectName ascending
                              where projectsModel.Status == true
                              select new SelectListItem
                              {
                                  Text = projectsModel.ProjectName,
                                  Value = projectsModel.ProjectId.ToString()

                              }).ToList();

            listofdata.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });
            return listofdata;

        }



        public List<SelectListItem> GetAssignedProjectListWithoutSelect(int? userId)
        {
            var listofdata = (from assignedProject in _bugPointContext.AssignedProjectModel
                              where assignedProject.UserId == userId
                              join projectsModel in _bugPointContext.ProjectsModel on assignedProject.ProjectId equals projectsModel.ProjectId
                              orderby projectsModel.ProjectName ascending
                              where projectsModel.Status == true

                              select new SelectListItem
                              {
                                  Text = projectsModel.ProjectName,
                                  Value = projectsModel.ProjectId.ToString()

                              }).ToList();

            return listofdata;

        }

    }
}