using BugPoint.Data.Project.Queries;
using BugPoint.Data.RoleMaster.Queries;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.ViewModel.Assigned;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;

using BugPoint.Data.Assigned.Command;
using BugPoint.Data.Assigned.Queries;
using BugPoint.Model.Assigned;
using BugPoint.ViewModel.RoleMaster;
using Microsoft.AspNetCore.Http;

namespace BugPoint.Application.Areas.Administration.Controllers
{
    [Area("Administration")]
    [SessionTimeOut]
    [AuthorizeAdminAttribute]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class AssignProjectController : Controller
    {
        private readonly IProjectQueries _projectQueries;
        private readonly IRoleQueries _roleQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IAssignProjectCommand _assignProjectCommand;
        private readonly IAssignProjectQueries _assignProjectQueries;
        private readonly INotificationService _notificationService;
        public AssignProjectController(
            IProjectQueries projectQueries,
            IRoleQueries roleQueries,
            IUserMasterQueries userMasterQueries,
            IAssignProjectCommand assignProjectCommand,
            IAssignProjectQueries assignProjectQueries, INotificationService notificationService)
        {
            _projectQueries = projectQueries;
            _roleQueries = roleQueries;
            _userMasterQueries = userMasterQueries;
            _assignProjectCommand = assignProjectCommand;
            _assignProjectQueries = assignProjectQueries;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Process()
        {
            var assignedProject = new RequestAssignedProjectViewModel()
            {
                ListofRoles = _roleQueries.GetAllActiveRoles()
            };

            return View(assignedProject);
        }

        [HttpPost]
        public IActionResult Process(AssignedProjectViewModel assignedProject)
        {
            if (_assignProjectQueries.CheckProjectAlreadyAssigned(assignedProject.ProjectId, assignedProject.UserId))
            {
                return Json(new { status = "Validation" });
            }

            var assignedProjectModel = new AssignedProjectModel()
            {
                AssignedProjectId = 0,
                CreatedOn = DateTime.Now,
                ProjectId = assignedProject.ProjectId,
                RoleId = assignedProject.RoleId,
                UserId = assignedProject.UserId,
                Status = true,
                CreatedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId)
            };

            var result = _assignProjectCommand.Add(assignedProjectModel);

            if (result > 0)
            {
                return Json(new { status = "Success" });
            }
            else
            {
                return Json(new { status = "Failed" });
            }


        }

        public IActionResult GetProjects(string projectname)
        {
            if (!string.IsNullOrEmpty(projectname))
            {
                var projectlist = _projectQueries.GetProjectbyProjectName(projectname);
                return Json(projectlist);
            }
            else
            {
                return Json(new List<SelectListItem>());
            }
        }

        public IActionResult GetUsernames(string roleId, string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var projectlist = _userMasterQueries.GetUserNameListbyUsername(username, Convert.ToInt32(roleId));
                return Json(projectlist);
            }
            else
            {
                return Json(new List<SelectListItem>());
            }
        }

        [HttpPost]
        public IActionResult GridAllAssignedProjects()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var projectId = string.IsNullOrEmpty(Request.Form["projectId"].FirstOrDefault()) ? "0" : Request.Form["projectId"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var querydata = _assignProjectQueries.ShowAllAssignedProjects(sortColumn, sortColumnDirection, searchValue, Convert.ToInt32(projectId));
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

        public JsonResult RemoveProjectAccess(RequestRemoveAssignedProject requestRemove)
        {
            try
            {
                if (requestRemove.AssignedProjectId == null)
                {
                    return Json(new { Result = "failed", Message = "Something went wrong. Please Try Once Again After Sometime." });
                }

                var projectdetails = _assignProjectQueries.GetAssignedProject(requestRemove.AssignedProjectId);

                var tempstatus = false;

                switch (requestRemove.AccessType)
                {
                    case "A":
                        tempstatus = true;
                        break;
                    case "R":
                        tempstatus = false;
                        break;
                }

                projectdetails.Status = tempstatus;

                projectdetails.ModifiedOn = DateTime.Now;
                projectdetails.ModifiedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                var result = _assignProjectCommand.Update(projectdetails);
                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message", "Removed Assigned Project Successfully!");
                    return Json(new { Result = "success" });
                }
                else
                {
                    return Json(new { Result = "failed", Message = "Cannot Delete" });
                }
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }

        public JsonResult DeleteProjectAccess(RequestDeleteAssignedProject requestdelete)
        {
            try
            {
                if (requestdelete.AssignedProjectId == null || requestdelete.RoleId == null)
                {
                    return Json(new { Result = "failed", Message = "Something went wrong. Please Try Once Again After Sometime." });
                }

                var checkresult = _assignProjectQueries.CheckIsUserAssignedAlreadyinUse(requestdelete.AssignedProjectId, requestdelete.RoleId);

                if (checkresult == false)
                {
                    var assignedProject = _assignProjectQueries.GetAssignedProject(requestdelete.AssignedProjectId);
                    var result = _assignProjectCommand.Delete(assignedProject);
                    if (result > 0)
                    {
                        return Json(new { Result = "success", Message = "Deleted Assigned Project Successfully" });
                    }
                    else
                    {
                        return Json(new { Result = "failed", Message = "Cannot Delete" });
                    }
                }
                else
                {
                    return Json(new { Result = "failed", Message = "Cannot Delete Assigned Project because it is in Use." });
                }
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }
    }
}
