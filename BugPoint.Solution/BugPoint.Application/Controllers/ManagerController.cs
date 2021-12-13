using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Common;
using BugPoint.Data.Assigned.Queries;
using BugPoint.Data.Bugs.Queries;
using BugPoint.Data.Charts.Queries;
using BugPoint.Data.Masters.Queries;
using BugPoint.Data.Project.Queries;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.Model.Bugs;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.BugsList;
using BugPoint.ViewModel.Charts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    [AuthorizeProjectManager]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class ManagerController : Controller
    {
        private readonly IBugQueries _bugQueries;
        private readonly IProjectQueries _projectQueries;
        private readonly IChartsQueries _chartsQueries;
        private readonly IProjectComponentQueries _projectComponentQueries;
        private readonly IMastersQueries _mastersQueries;
        private readonly IAssignProjectQueries _assignProjectQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        public ManagerController(IProjectQueries projectQueries,
            IChartsQueries chartsQueries,
            IProjectComponentQueries projectComponentQueries,
            IMastersQueries mastersQueries,
            IAssignProjectQueries assignProjectQueries, IBugQueries bugQueries, IUserMasterQueries userMasterQueries)
        {
            _projectQueries = projectQueries;
            _chartsQueries = chartsQueries;
            _projectComponentQueries = projectComponentQueries;
            _mastersQueries = mastersQueries;
            _assignProjectQueries = assignProjectQueries;
            _bugQueries = bugQueries;
            _userMasterQueries = userMasterQueries;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var reportManager = new ReportManagerChartTableViewModel()
            {
                ListofProjects = _projectQueries.GetProjectList()
            };

            return View(reportManager);
        }


        [HttpGet]
        public IActionResult AllBugs(int? projectId,
            int? projectComponentId,
            int? priorityId,
            int? severityId,
            int? statusId,
            int? versionId,
            int? operatingSystemId,
            int? hardwareId,
            int? browserId,
            int? webFrameworkId,
            int? testedOnId,
            int? bugTypeId,
            int? reportersUserId,
            int? developersUserId,
            int? page = 1)
        {
            if (page < 0)
            {
                page = 1;
            }

            var bugListView = new AllBugListViewModel();

            if (projectId == null)
            {
                var defaultproject = _projectQueries.GetProjectList().FirstOrDefault()?.Value;
                bugListView.ProjectId = Convert.ToInt32(defaultproject);
                projectId = Convert.ToInt32(defaultproject);
            }

            bugListView.ListofStatus = _mastersQueries.ListofStatus();
            bugListView.ListofProjects = _projectQueries.GetProjectList();
            bugListView.ListofComponents = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "", Text = "-----Select-----"
                }
            };
            bugListView.ListofSeverity = _mastersQueries.ListofSeverity();
            bugListView.ListofPriority = _mastersQueries.ListofPriority();
            bugListView.ListofVersion = _mastersQueries.ListofVersions();
            bugListView.ListofOperatingSystem = _mastersQueries.ListofOperatingSystem();
            bugListView.ListofHardware = _mastersQueries.ListofHardware();
            bugListView.ListofBrowser = _mastersQueries.ListofBrowser();
            bugListView.ListofWebFramework = _mastersQueries.ListofWebFramework();
            bugListView.ListofTestedOn = _mastersQueries.ListofEnvironments();
            bugListView.ListofBugType = _mastersQueries.ListofBugTypes();

            bugListView.ListofReporters = _assignProjectQueries.GetTestersandTeamLeadAssignedtoProject(projectId);
            bugListView.ListofDevelopers = _assignProjectQueries.GetDevelopersandTeamLeadAssignedtoProject(projectId);



            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;

            int totalbugsCount = 0;

            totalbugsCount = _bugQueries.GetManagerBugsCount(projectId,
                projectComponentId,
                priorityId,
                severityId,
                statusId,
                versionId,
                operatingSystemId,
                hardwareId,
                browserId,
                webFrameworkId,
                testedOnId,
                bugTypeId,
                reportersUserId,
                developersUserId,
                page);

            var buglist = _bugQueries.GetManagerBugsList(projectId,
                projectComponentId,
                priorityId,
                severityId,
                statusId,
                versionId,
                operatingSystemId,
                hardwareId,
                browserId,
                webFrameworkId,
                testedOnId,
                bugTypeId,
                reportersUserId,
                developersUserId,
                page,pageSize);

            var buglistPagedList = new StaticPagedList<BugListGrid>(buglist, pageIndex + 1, pageSize, totalbugsCount);

            bugListView.BugListGrid = buglistPagedList;
            bugListView.PageSize = page;
            bugListView.ProjectId = projectId;
            bugListView.BrowserId = browserId;
            bugListView.DevelopersUserId = developersUserId;
            bugListView.ReportersUserId = reportersUserId;
            bugListView.SeverityId = severityId;
            bugListView.BugTypeId = bugTypeId;
            bugListView.HardwareId = hardwareId;
            bugListView.OperatingSystemId = operatingSystemId;
            bugListView.PriorityId = priorityId;
            bugListView.StatusId = statusId;
            bugListView.ProjectComponentId = projectComponentId;
            bugListView.TestedOnId = testedOnId;
            bugListView.VersionId = versionId;
            bugListView.WebFrameworkId = webFrameworkId;


            return View(bugListView);

        }


        public IActionResult GetProjectComponents(RequestProjectComponent requestProject)
        {
            var listofprojects = _projectComponentQueries.GetProjectComponentsList(requestProject.ProjectId);
            return Json(new { projectcollection = listofprojects });
        }

        public IActionResult GetPieChartData(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var chartsdata = _chartsQueries.GetReporterLeadPieChartbyUserId(requestProject.ProjectId);

            var bugcount = (from temp in chartsdata
                            select temp.TotalCount).ToList();

            var status = (from temp in chartsdata
                          select temp.StatusName).ToList();

            var listofcolors = new List<string>();

            var listcolortoshow = new List<string>();

            foreach (var value in chartsdata)
            {
                if (value.StatusName == "Open")
                {
                    listcolortoshow.Add("#99ccff");
                }
                else if (value.StatusName == "Confirmed")
                {
                    listcolortoshow.Add("#C9AAFA");
                }
                else if (value.StatusName == "In-Progress")
                {
                    listcolortoshow.Add("#f96987");
                }
                else if (value.StatusName == "Re-Opened")
                {
                    listcolortoshow.Add("#ffdc73");
                }
                else if (value.StatusName == "Resolved")
                {
                    listcolortoshow.Add("#83e7a8");
                }

                else if (value.StatusName == "InTesting")
                {
                    listcolortoshow.Add("#FDBC84");
                }

                else if (value.StatusName == "Closed")
                {
                    listcolortoshow.Add("#9ccc34");
                }
                else if (value.StatusName == "On-Hold")
                {
                    listcolortoshow.Add("#FFDF7D");
                }
                else if (value.StatusName == "Rejected")
                {
                    listcolortoshow.Add("#ff7474");
                }

                else if (value.StatusName == "Reply")
                {
                    listcolortoshow.Add("#ff9936");
                }
                else if (value.StatusName == "Duplicate")
                {
                    listcolortoshow.Add("#C5D3E3");
                }
                else if (value.StatusName == "UnConfirmed")
                {
                    listcolortoshow.Add("#FFE9D2");
                }
            }

            ViewBag.BugCount_List = string.Join(",", bugcount);
            ViewBag.Statusname_List = string.Join(", ", status.Select(item => "\"" + item + "\""));
            var joined = string.Join(", ", listcolortoshow.Select(item => "\"" + item + "\""));
            ViewBag.Color_List = joined;

            var pieRoot = new PieRoot()
            {
                labels = status,
                datasets = new List<PieDataset>()
                {
                   new PieDataset()
                   {
                      backgroundColor = listcolortoshow,
                      borderWidth =2,
                      data =bugcount,

                   }
                }
            };

            return Json(pieRoot);
        }

        public IActionResult GetPriorityChartData(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var chartsPrioritydata = _chartsQueries.GetReporterLeadPriorityPieChartbyUserId(requestProject.ProjectId);

            var bugPrioritycount = (from temp in chartsPrioritydata
                                    select temp.TotalCount).ToList();

            var prioritylist = (from temp in chartsPrioritydata
                                select temp.PriorityName).ToList();


            var listPrioritycolortoshow = new List<string>();

            foreach (var value in chartsPrioritydata)
            {
                if (value.PriorityName == "Urgent")
                {
                    listPrioritycolortoshow.Add("#f07676");
                }
                if (value.PriorityName == "High")
                {
                    listPrioritycolortoshow.Add("#ff8100");
                }
                if (value.PriorityName == "Medium")
                {
                    listPrioritycolortoshow.Add("#ffd700");
                }
                if (value.PriorityName == "Low")
                {
                    listPrioritycolortoshow.Add("#cceaff");
                }
            }

            var pieRoot = new PieRoot()
            {
                labels = prioritylist,
                datasets = new List<PieDataset>()
                {
                    new PieDataset()
                    {
                        backgroundColor = listPrioritycolortoshow,
                        borderWidth =2,
                        data =bugPrioritycount,

                    }
                }
            };
            return Json(pieRoot);
        }

        public IActionResult GetPieOpenCloseChartData(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            #region Priority Pie Chart
            var chartsOpenclosedata = _chartsQueries.GetReporterLeadBugsCountPieChartbyUserId(requestProject.ProjectId);

            var listofcount = new List<int>();
            var listofStatusnames = new List<string>()
            {
                "Open","Closed"
            };


            var bugOpencount = (from temp in chartsOpenclosedata
                                select temp.Open).FirstOrDefault();
            listofcount.Add(bugOpencount);

            var bugClosecount = (from temp in chartsOpenclosedata
                                 select temp.Closed).FirstOrDefault();

            listofcount.Add(bugClosecount);

            var listOpenClosecolortoshow = new List<string>()
            {
                "#99ccff","#9ccc34"
            };

            ViewBag.BugOpenCloseCount_List = string.Join(",", listofcount);

            ViewBag.OpenCloseLabels_List = string.Join(", ", listofStatusnames.Select(item => "\"" + item + "\""));

            var joinedOpenclose = string.Join(", ", listOpenClosecolortoshow.Select(item => "\"" + item + "\""));
            ViewBag.OpenCloseColor_List = joinedOpenclose;

            #endregion


            var pieRoot = new PieRoot()
            {
                labels = listofStatusnames,
                datasets = new List<PieDataset>()
                {
                    new PieDataset()
                    {
                        backgroundColor = listOpenClosecolortoshow,
                        borderWidth =2,
                        data =listofcount,

                    }
                }
            };
            return Json(pieRoot);
        }

        public IActionResult GetStatusWiseBugsCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofStatus = _chartsQueries.GetLeadStatusWiseBugCount(user, requestProject.ProjectId);
            return Json(listofStatus);
        }

        public IActionResult GetTesterTeamsBugsCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var dataCount = _chartsQueries.GetTesterTeamsBugsCount(requestProject.ProjectId);
            return PartialView("_CommonStatusTables", dataCount);
        }

        public IActionResult GetBrowserChartData(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var browserdata = _chartsQueries.GetBrowserNamesofTestedBugs(requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Browsers",
                ReporterCommonViewModel = browserdata
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }

        public IActionResult GetSeveritywiseCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofSeverity = _chartsQueries.GetLeadSeveritywiseCount(requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Severity",
                ReporterCommonViewModel = listofSeverity
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }

        public IActionResult GetHardwareDetails(RequestProjectReporter requestProject)
        {
            var listofHardware = _chartsQueries.GetHardwareDetails(requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Hardware",
                ReporterCommonViewModel = listofHardware
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }

        public IActionResult GetVersionDetails(RequestProjectReporter requestProject)
        {
            var listofversion = _chartsQueries.GetVersionDetails(requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Versions",
                ReporterCommonViewModel = listofversion
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }

        public IActionResult GetOperatingSystemDetails(RequestProjectReporter requestProject)
        {
            var listofversion = _chartsQueries.GetOperatingSystemDetails(requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Operating Systems",
                ReporterCommonViewModel = listofversion
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }

        public IActionResult GetWebFrameworkDetails(RequestProjectReporter requestProject)
        {
            var listofversion = _chartsQueries.GetWebFrameworkDetailsProjectWise(requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Framework",
                ReporterCommonViewModel = listofversion
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }

        public IActionResult GetBugTypeProjectwiseCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofBugType = _chartsQueries.GetLeadBugTypeProjectwiseCount(requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "BugType Type",
                ReporterCommonViewModel = listofBugType
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }

        public IActionResult GetTestedEnvironmentProjectwiseCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofTestedEnvironment = _chartsQueries.GetLeadTestedEnvironmentProjectwiseCount(requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Environment Type",
                ReporterCommonViewModel = listofTestedEnvironment
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }

        public IActionResult GetDeveloperTeamsBugsCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var dataCount = _chartsQueries.GetDeveloperTeamsBugsCount(requestProject.ProjectId);
            return PartialView("_CommonStatusTables", dataCount);
        }

        public IActionResult GetTotalBugsCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var dataCount = _chartsQueries.GetTotalBugStatusCountProjectWise(requestProject.ProjectId);
            return PartialView("_CommonTotalCountTables", dataCount);
        }



        [HttpGet]
        public IActionResult BugDetails(long? id)
        {

            var bugDetailViewModel = _bugQueries.GetBugDetailsbyBugId(id);
            ViewBag.CurrentBugStatus = (int)StatusHelper.Status.CLOSED;

            var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
            ViewBag.Currentrole = currentrole;

            var displayBugViewModel = new DisplayBugViewModel()
            {
                BugDetailViewModel = bugDetailViewModel,
                ListofAttachments = _bugQueries.GetListAttachmentsBybugId(id),
                BugReplyViewModel = new BugReplyViewModel()
                {
                    BugId = bugDetailViewModel.BugId,
                    ListofStatus = _mastersQueries.ListofReportStatus()
                },
                ViewBugReplyMainModel = new ViewBugReplyMainModel()
                {
                    ListofTicketreply = _bugQueries.ListofHistoryTicketReplies(id),
                    ListofReplyAttachment = new List<ReplyAttachmentModel>()
                },
                ExpressChangesViewModel = new ExpressChangesViewModel()
                {
                    ListofPriority = _mastersQueries.ListofPriority(),
                    PriorityId = bugDetailViewModel.PriorityId,
                    AssignedToId = bugDetailViewModel.AssignedToId,
                    ListofUsers = _userMasterQueries.GetListofDevelopers(Convert.ToInt32(bugDetailViewModel.ProjectId)),
                    AssignedTo = bugDetailViewModel.AssignedTo,
                    Priority = bugDetailViewModel.Priority
                }
            };


            return View(displayBugViewModel);
        }

        public IActionResult GetBugActivities(RequestBugReopen requestBug)
        {
            var listofbugHistory = _bugQueries.GetBugHistorybyBugId(requestBug.BugId);
            return PartialView("_BugActivities", listofbugHistory);
        }
    }
}
