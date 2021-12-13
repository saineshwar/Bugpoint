using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.Assigned.Queries;
using BugPoint.Data.Bugs.Queries;
using BugPoint.Data.Masters.Queries;
using BugPoint.Data.Project.Queries;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.BugsList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class BugListController : Controller
    {
        private readonly IBugQueries _bugQueries;
        private readonly IProjectQueries _projectQueries;
        private readonly IProjectComponentQueries _projectComponentQueries;
        private readonly IMastersQueries _mastersQueries;
        private readonly IAssignProjectQueries _assignProjectQueries;
        private readonly INotificationService _notificationService;
        public BugListController(IBugQueries bugQueries,
            IProjectQueries projectQueries,
            IProjectComponentQueries projectComponentQueries,
            IMastersQueries mastersQueries,
            IAssignProjectQueries assignProjectQueries,
            INotificationService notificationService)
        {
            _bugQueries = bugQueries;
            _projectQueries = projectQueries;
            _projectComponentQueries = projectComponentQueries;
            _mastersQueries = mastersQueries;
            _assignProjectQueries = assignProjectQueries;
            _notificationService = notificationService;
        }

        [AuthorizeTester]
        public IActionResult Show(int? projectId,
            int? projectComponentId, 
            int? priorityId, 
            int? severityId, 
            int? statusId,
            int? assignedtoId, 
            int? page = 1)
        {

            if (page < 0)
            {
                page = 1;
            }

            var bugListView = new BugListViewModel();
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var projectlistcount = _projectQueries.GetAssignedProjectListWithoutSelect(user);


            if (projectlistcount.Count == 0)
            {
                _notificationService.DangerNotification("Message", $"Project is not assigned. Please Contact Admin Team!");
            }

            if (projectId == null)
            {
                var defaultproject = _projectQueries.GetAssignedProjectListWithoutSelect(user).FirstOrDefault()?.Value;
                bugListView.ProjectId = Convert.ToInt32(defaultproject);
                projectId = Convert.ToInt32(defaultproject);
            }


            bugListView.ListofProjects = projectlistcount;
            bugListView.ListofSeverity = _mastersQueries.ListofSeverity();
            bugListView.ListofPriority = _mastersQueries.ListofPriority();
            bugListView.ListofStatus = _mastersQueries.ListofStatus();
            bugListView.ListofDeveloperandLead = _assignProjectQueries.GetDevelopersandTeamLeadAssignedtoProject(projectId);
            bugListView.ListofComponents = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "", Text = "-----Select-----"
                }
            };

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;

            int totalbugsCount = 0;

            totalbugsCount = _bugQueries.GetReportersBugsCount(user, projectId, projectComponentId, priorityId,
                severityId, statusId, assignedtoId);

            var buglist = _bugQueries.GetReportersBugsList(user, projectId, projectComponentId, priorityId, severityId,
                statusId, assignedtoId, page, pageSize).ToList();

            var buglistPagedList = new StaticPagedList<BugListGrid>(buglist, pageIndex + 1, pageSize, totalbugsCount);

            bugListView.BugListGrid = buglistPagedList;
            bugListView.PageSize = page;
            bugListView.AssignedtoId = assignedtoId;
            bugListView.ProjectId = projectId;
            bugListView.PriorityId = priorityId;
            bugListView.SeverityId = severityId;
            bugListView.StatusId = statusId;
            bugListView.ProjectComponentId = projectComponentId;
            return View(bugListView);

        }


        [AuthorizeTester]
        public IActionResult ShowRecentActivities(int? projectId, 
            int? projectComponentId, 
            int? priorityId,
            int? severityId, 
            int? statusId, 
            int? assignedtoId,
            int? page = 1)
        {

            if (page < 0)
            {
                page = 1;
            }

            var bugListView = new ReportedRecentBugListViewModel();
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var projectlistcount = _projectQueries.GetAssignedProjectListWithoutSelect(user);


            if (projectlistcount.Count == 0)
            {
                _notificationService.DangerNotification("Message", $"Project is not assigned. Please Contact Admin Team!");
            }

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;

            int totalbugsCount = 0;

            totalbugsCount = _bugQueries.GetReportersBugsCountLastSevenDays(user);

            var buglist = _bugQueries.GetReportersBugsListLastSevenDays(user, page, pageSize).ToList();

            var buglistPagedList = new StaticPagedList<BugListGrid>(buglist, pageIndex + 1, pageSize, totalbugsCount);

            bugListView.BugListGrid = buglistPagedList;
            bugListView.Page = page;
            return View(bugListView);

        }



        [AuthorizeTesterLead]
        public IActionResult AllReportedBugs(int? projectId,
            int? projectComponentId,
            int? priorityId,
            int? severityId,
            int? statusId,
            int? reportersUserId, int? page = 1)
        {
            if (page < 0)
            {
                page = 1;
            }

            var bugListView = new ReportedBugListViewModel();
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            if (projectId == null)
            {
                var defaultproject = _projectQueries.GetAssignedProjectListWithoutSelect(user).FirstOrDefault()?.Value;
                bugListView.ProjectId = Convert.ToInt32(defaultproject);
                projectId = Convert.ToInt32(defaultproject);
            }

            bugListView.ListofProjects = _projectQueries.GetAssignedProjectList(user);
            bugListView.ListofReporters = _assignProjectQueries.GetTestersandTeamLeadAssignedtoProject(projectId);
            bugListView.ListofSeverity = _mastersQueries.ListofSeverity();
            bugListView.ListofPriority = _mastersQueries.ListofPriority();
            bugListView.ListofStatus = _mastersQueries.ListofStatus();
            bugListView.ListofComponents = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "", Text = "-----Select-----"
                }
            };

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;

            int totalbugsCount = 0;



            totalbugsCount = _bugQueries.GetMyReportersBugsCount(user, projectId, projectComponentId, priorityId,
                severityId, reportersUserId, statusId);

            var buglist = _bugQueries.GetmyReportersBugsList(user, projectId, projectComponentId, priorityId, severityId,
                statusId, reportersUserId, page, pageSize).ToList();


            var buglistPagedList = new StaticPagedList<BugListGrid>(buglist, pageIndex + 1, pageSize, totalbugsCount);

            bugListView.BugListGrid = buglistPagedList;
            bugListView.PageSize = page;

            bugListView.ProjectId = projectId;
            bugListView.PriorityId = priorityId;
            bugListView.SeverityId = severityId;
            bugListView.StatusId = statusId;
            bugListView.ReportersUserId = reportersUserId;
            bugListView.ProjectComponentId = projectComponentId;
            return View(bugListView);

        }



        [AuthorizeTesterLead]
        public IActionResult AllRecentActivities(int? projectId,
            int? projectComponentId,
            int? priorityId,
            int? severityId,
            int? statusId,
            int? reportersUserId, int? page = 1)
        {
            if (page < 0)
            {
                page = 1;
            }

            var bugListView = new ReportedRecentBugListViewModel();
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;

            int totalbugsCount = 0;
            totalbugsCount = _bugQueries.GetMyReportersBugsCountLastSevenDays(user);

            var buglist = _bugQueries.GetmyReportersBugsListLastSevenDays(user,  page, pageSize).ToList();


            var buglistPagedList = new StaticPagedList<BugListGrid>(buglist, pageIndex + 1, pageSize, totalbugsCount);

            bugListView.BugListGrid = buglistPagedList;
            bugListView.Page = page;
            return View(bugListView);

        }

        public IActionResult GetProjectComponents(RequestProjectComponent requestProject)
        {
            var listofprojects = _projectComponentQueries.GetProjectComponentsList(requestProject.ProjectId);
            return Json(new { projectcollection = listofprojects });
        }

        public IActionResult GetAssignedtoProjectReporter(RequestProjectReporter requestProject)
        {
            var listofusers =
                _assignProjectQueries.GetTestersandTeamLeadAssignedtoProject(requestProject.ProjectId);
            return Json(listofusers);
        }

        public IActionResult GetDeveloperList(RequestProjectComponent requestProject)
        {
            var roleValue = Convert.ToInt32(HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
            var listofusers = _assignProjectQueries.GetDevelopersandTeamLeadAssignedtoProject(requestProject.ProjectId);
            return Json(new { listofusers = listofusers });
        }

    }
}
