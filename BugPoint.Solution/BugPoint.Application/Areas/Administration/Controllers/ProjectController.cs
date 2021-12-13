using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.Project.Command;
using BugPoint.Data.Project.Queries;
using BugPoint.Model.Project;
using BugPoint.ViewModel.Project;
using Microsoft.AspNetCore.Http;

namespace BugPoint.Application.Areas.Administration.Controllers
{
    [Area("Administration")]
    [SessionTimeOut]
    [AuthorizeAdminAttribute]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class ProjectController : Controller
    {
        private readonly IProjectCommand _projectCommand;
        private readonly IProjectQueries _projectQueries;
        private readonly INotificationService _notificationService;
        public ProjectController(IProjectCommand projectCommand, IProjectQueries projectQueries, INotificationService notificationService)
        {
            _projectCommand = projectCommand;
            _projectQueries = projectQueries;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var project = new ProjectsViewModel()
            {
                Status = true
            };
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectsViewModel projectsModel)
        {
            if (ModelState.IsValid)
            {
                if (_projectQueries.CheckProjectNameExists(projectsModel.ProjectName))
                {
                    _notificationService.DangerNotification("Error", "Project Name Already Exists!");
                    return View(projectsModel);
                }

                var project = new ProjectsModel()
                {
                    CreatedOn = DateTime.Now,
                    ProjectId = 0,
                    Status = true,
                    CreatedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId),
                    ProjectDescription = projectsModel.ProjectDescription,
                    ProjectName = projectsModel.ProjectName
                };

                var result = _projectCommand.Add(project);

                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(projectsModel);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllProjects()
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
                var rolesdata = _projectQueries.ShowAllProjects(sortColumn, sortColumnDirection, searchValue);
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

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Project");
                }
                var project = _projectQueries.EditProject(id);
                return View(project);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditProjectViewModel editProjectvm)
        {
            if (ModelState.IsValid)
            {
                var project = _projectQueries.GetProjectDetails(editProjectvm.ProjectId);

                if (project.ProjectName != editProjectvm.ProjectName)
                {
                    if (_projectQueries.CheckProjectNameExists(editProjectvm.ProjectName))
                    {
                        _notificationService.DangerNotification("Error", "Project Name Already Exists!");
                        return View(editProjectvm);
                    }
                }

                project.ModifiedOn = DateTime.Now;
                project.ProjectId = editProjectvm.ProjectId;
                project.Status = editProjectvm.Status;
                project.ModifiedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                project.ProjectDescription = editProjectvm.ProjectDescription;
                project.ProjectName = editProjectvm.ProjectName;

                var result = _projectCommand.Update(project);
                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message", "Project Updated successfully!");
                    return RedirectToAction("Index", "Project");
                }
            }

            return View(editProjectvm);
        }

    }
}
