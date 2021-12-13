using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
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
    public class MyBugListController : Controller
    {
        private readonly IBugQueries _bugQueries;
        private readonly IProjectQueries _projectQueries;
        private readonly IProjectComponentQueries _projectComponentQueries;
        private readonly IMastersQueries _mastersQueries;
        private readonly IAssignProjectQueries _assignProjectQueries;
        public MyBugListController(IBugQueries bugQueries,
            IProjectQueries projectQueries,
            IProjectComponentQueries projectComponentQueries,
            IMastersQueries mastersQueries,
            IAssignProjectQueries assignProjectQueries)
        {
            _bugQueries = bugQueries;
            _projectQueries = projectQueries;
            _projectComponentQueries = projectComponentQueries;
            _mastersQueries = mastersQueries;
            _assignProjectQueries = assignProjectQueries;
        }

        [AuthorizeDeveloper]
        public IActionResult Show(int? projectId, int? projectComponentId, int? priorityId, int? severityId, int? statusId, int? page = 1)
        {

            if (page < 0)
            {
                page = 1;
            }

            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var bugListView = new BugListViewModel();

            if (projectId == null)
            {
                var defaultproject = _projectQueries.GetAssignedProjectListWithoutSelect(user).FirstOrDefault()?.Value;
                bugListView.ProjectId = Convert.ToInt32(defaultproject);
                projectId = Convert.ToInt32(defaultproject);
            }


            bugListView.ListofProjects = _projectQueries.GetAssignedProjectListWithoutSelect(user);
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
            totalbugsCount = _bugQueries.GetMyBugsCount(user, projectId, projectComponentId, priorityId, severityId, statusId);
            var buglist = _bugQueries.GetMyBugsList(user, projectId, projectComponentId, priorityId, severityId, statusId, page, pageSize).ToList();
            var buglistPagedList = new StaticPagedList<BugListGrid>(buglist, pageIndex + 1, pageSize, totalbugsCount);

            bugListView.BugListGrid = buglistPagedList;
            bugListView.PageSize = page;

            bugListView.ProjectId = projectId;
            bugListView.PriorityId = priorityId;
            bugListView.SeverityId = severityId;
            bugListView.StatusId = statusId;

            bugListView.ProjectComponentId = projectComponentId;
            return View(bugListView);

        }



        [AuthorizeDeveloper]
        public IActionResult ShowRecentActivities(int? page = 1)
        {

            if (page < 0)
            {
                page = 1;
            }

            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var bugListView = new ReportedRecentBugListViewModel();

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;
            int totalbugsCount = 0;
            totalbugsCount = _bugQueries.GetMyBugsCountLastSevenDays(user);
            var buglist = _bugQueries.GetMyBugsListLastSevenDays(user, page, pageSize).ToList();
            var buglistPagedList = new StaticPagedList<BugListGrid>(buglist, pageIndex + 1, pageSize, totalbugsCount);

            bugListView.BugListGrid = buglistPagedList;
            bugListView.Page = page;
            return View(bugListView);

        }





        [AuthorizeDeveloperLead]
        public IActionResult AllReportedBugs(int? projectId,
            int? projectComponentId,
            int? priorityId,
            int? severityId,
            int? statusId,
            int? devId,
            int? page = 1)
        {

            if (page < 0)
            {
                page = 1;
            }

            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var bugListView = new DeveloperBugListViewModel();

            if (projectId == null)
            {
                var defaultproject = _projectQueries.GetAssignedProjectListWithoutSelect(user).FirstOrDefault()?.Value;
                bugListView.ProjectId = Convert.ToInt32(defaultproject);
                projectId = Convert.ToInt32(defaultproject);
            }


            bugListView.ListofProjects = _projectQueries.GetAssignedProjectListWithoutSelect(user);
            bugListView.ListofDevelopers = _assignProjectQueries.GetTestersandDevelopersAssignedtoProject(projectId, (int)RolesHelper.Roles.Developer);
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
            totalbugsCount = _bugQueries.GetMyTeamsBugsCount(user, projectId, projectComponentId, priorityId, severityId, statusId, devId);
            var buglist = _bugQueries.GetMyTeamsBugsList(user, projectId, projectComponentId, priorityId, severityId, statusId, devId, page, pageSize).ToList();

            var buglistPagedList = new StaticPagedList<BugListGrid>(buglist, pageIndex + 1, pageSize, totalbugsCount);

            bugListView.BugListGrid = buglistPagedList;
            bugListView.PageSize = page;

            bugListView.ProjectId = projectId;
            bugListView.PriorityId = priorityId;
            bugListView.SeverityId = severityId;
            bugListView.StatusId = statusId;
            bugListView.DevId = devId;
            bugListView.ProjectComponentId = projectComponentId;
            return View(bugListView);

        }

        [AuthorizeDeveloperLead]
        public IActionResult AllRecentActivities(int? projectId,
         int? projectComponentId,
         int? priorityId,
         int? severityId,
         int? statusId,
         int? devId,
         int? page = 1)
        {

            if (page < 0)
            {
                page = 1;
            }

            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var bugListView = new ReportedRecentBugListViewModel();

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;
            int totalbugsCount = 0;
            totalbugsCount = _bugQueries.GetMyTeamsBugsCountLastSevenDays(user);
            var buglist = _bugQueries.GetMyTeamsBugsListLastSevenDays(user,  page, pageSize).ToList();

            var buglistPagedList = new StaticPagedList<BugListGrid>(buglist, pageIndex + 1, pageSize, totalbugsCount);

            bugListView.BugListGrid = buglistPagedList;
            bugListView.Page = page;
            return View(bugListView);

        }


        public IActionResult GetProjectComponentsDeveloper(RequestProjectComponent requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofprojects = _projectComponentQueries.GetProjectComponentsListByUserId(requestProject.ProjectId, user);

            return Json(new { projectcollection = listofprojects });
        }

        public IActionResult GetProjectComponentsDeveloperLead(RequestProjectComponent requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofprojects = _projectComponentQueries.GetProjectComponentsList(requestProject.ProjectId);

            return Json(new { projectcollection = listofprojects });
        }



        public IActionResult GetAssignedtoProjectReporter(RequestProjectReporter requestProject)
        {
            var listofusers = _assignProjectQueries.GetTestersandDevelopersAssignedtoProject(requestProject.ProjectId, (int)RolesHelper.Roles.Developer);
            return Json(listofusers);
        }
    }
}
