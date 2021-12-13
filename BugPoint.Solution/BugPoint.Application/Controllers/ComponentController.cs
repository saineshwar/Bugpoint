using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.Assigned.Command;
using BugPoint.Data.Assigned.Queries;
using BugPoint.Data.Project.Queries;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.Model.Assigned;
using BugPoint.Services.MailHelper;
using BugPoint.ViewModel.Assigned;
using BugPoint.ViewModel.Bugs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    [AuthorizeDeveloperLead]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class ComponentController : Controller
    {
        private readonly IProjectQueries _projectQueries;
        private readonly IProjectComponentCommand _projectComponentCommand;
        private readonly IProjectComponentQueries _projectComponentQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IAssignProjectQueries _assignProjectQueries;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ComponentController> _logger;
        public ComponentController(IProjectQueries projectQueries,
            IProjectComponentCommand projectComponentCommand,
            IProjectComponentQueries projectComponentQueries,
            IUserMasterQueries userMasterQueries, 
            IAssignProjectQueries assignProjectQueries, 
            INotificationService notificationService, ILogger<ComponentController> logger)
        {
            _projectQueries = projectQueries;
            _projectComponentCommand = projectComponentCommand;
            _projectComponentQueries = projectComponentQueries;
            _userMasterQueries = userMasterQueries;
            _assignProjectQueries = assignProjectQueries;
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var projectComponent = new ProjectComponentViewModel()
            {
                ListofProjects = _projectQueries.GetAssignedProjectList(user),
                Status = true,
                ListofUsers = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "", Text = "-----Select-----"
                    }
                }
            };

            return View(projectComponent);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var projectComponent = _projectComponentQueries.GetProjectComponentDetailsByProjectId(id);
            projectComponent.ListofProjects = _projectQueries.GetAssignedProjectList(user);
            projectComponent.ListofUsers = _assignProjectQueries.GetDevelopersandTeamLeadAssignedtoProject(projectComponent.ProjectId);
            projectComponent.Status = true;
            return View(projectComponent);
        }

        [HttpPost]
        public IActionResult Edit(EditProjectComponentViewModel editProjectComponent)
        {

            var projectComponentmodel = _projectComponentQueries.ProjectComponentDetailsByProjectId(editProjectComponent.ProjectComponentId);
            projectComponentmodel.ComponentName = editProjectComponent.ComponentName;
            projectComponentmodel.ComponentDescription = editProjectComponent.ComponentDescription;
            projectComponentmodel.Status = editProjectComponent.Status;

            var result = _projectComponentCommand.Update(projectComponentmodel);

            if (result > 0)
            {
                _notificationService.SuccessNotification("Message", "Project Component Updated Successfully.");
                return RedirectToAction("Edit", new {id = editProjectComponent.ProjectComponentId});
            }
            else
            {
                _notificationService.DangerNotification("Message", "Something Went Wrong Please Refresh Page and Try Once Again!");
                return RedirectToAction("Edit", new { id = editProjectComponent.ProjectComponentId });
            }
        }


        [HttpPost]
        public IActionResult Add(RequestProjectComponentViewModel projectComponent)
        {
            if (string.IsNullOrEmpty(projectComponent.ComponentName))
            {
                return Json(new { status = "validation", message = "Component Name Required!" });
            }
            if (projectComponent.ProjectId == null)
            {
                return Json(new { status = "validation", message = "ProjectId Required!" });
            }
            if (projectComponent.AssignBugto == null)
            {
                return Json(new { status = "validation", message = "AssignBugto Required" });
            }
            if (_projectComponentQueries.CheckProjectComponentExists(projectComponent.ProjectId.Value, projectComponent.ComponentName))
            {
                return Json(new { status = "validation", message = "Component Name Already Used!" });
            }

            var projectComponentmodel = new ProjectComponentModel()
            {
                ProjectId = projectComponent.ProjectId.Value,
                Status = true,
                ComponentDescription = projectComponent.ComponentDescription,
                ComponentName = projectComponent.ComponentName,
                CreatedOn = DateTime.Now,
                CreatedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId),
                ProjectComponentId = 0,
                AssignedTo = projectComponent.AssignBugto
            };

            var result = _projectComponentCommand.Add(projectComponentmodel);

            if (result > 0)
            {
                return Json(new { status = "success", message = "Component Added Successfully!" });
            }
            else
            {
                return Json(new { status = "failed", message = "Something Went Wrong Please Refresh Page and Try Once Again!" });
            }

        }

        [HttpPost]
        public IActionResult Delete(RequestDeleteComponent deleteComponent)
        {
            try
            {
                if (deleteComponent.ProjectComponentId == null)
                {
                    return Json(new { status = "validation", message = "Project ComponentId Required!" });
                }
                var result = _projectComponentCommand.Delete(deleteComponent.ProjectComponentId);
                if (result)
                {
                    return Json(new { status = "success", message = "Deleted Component Successfully!" });
                }
                else
                {
                    return Json(new { status = "failed", message = "Something Went Wrong Please Refresh Page and Try Once Again!" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Component Delete.");
                return Json(new { status = "failed", message = "Something Went Wrong Please Refresh Page and Try Once Again!" });
            }
        }


        [HttpPost]
        public IActionResult GridAllProjectComponents()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var projectId = Request.Form["projectId"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var querydata = _projectComponentQueries.ShowAllProjectComponents(sortColumn, sortColumnDirection, searchValue, Convert.ToInt32(projectId));
                recordsTotal = querydata.Count();
                var data = querydata.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult GetUserList(RequestProjectComponent requestProject)
        {
            var listofusers = _assignProjectQueries.GetDevelopersandTeamLeadAssignedtoProject(requestProject.ProjectId);
            return Json(new { listofusers = listofusers });
        }
    }
}
