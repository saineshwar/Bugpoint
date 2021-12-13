using System;
using BugPoint.Common;
using BugPoint.Data.Charts.Queries;
using BugPoint.Data.Project.Queries;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.Charts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using BugPoint.Application.Filters;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.ViewModel.Login;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class DashboardController : Controller
    {
        private readonly IChartsQueries _chartsQueries;
        private readonly IProjectQueries _projectQueries;
        
        public DashboardController(IChartsQueries chartsQueries, IProjectQueries projectQueries)
        {
            _chartsQueries = chartsQueries;
            _projectQueries = projectQueries;
        }

        [HttpGet]
        [AuthorizeDeveloper]
        public IActionResult Dashboard()
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var reportChartTableViewModel = new ReportChartTableViewModel()
            {
                ListofProjects = _projectQueries.GetAssignedProjectListWithoutSelect(user)
            };

            return View(reportChartTableViewModel);
        }

        [AuthorizeDeveloper]
        public IActionResult GetPieChartData(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var chartsdata = _chartsQueries.GetDeveloperPieChartbyUserId(user, requestProject.ProjectId);

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

        [AuthorizeDeveloper]
        public IActionResult GetPriorityChartData(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var chartsPrioritydata = _chartsQueries.GetDeveloperPriorityPieChartbyUserId(user, requestProject.ProjectId);

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

        [AuthorizeDeveloper]
        public IActionResult GetBarChart()
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            #region Project Wise Bar Chart
            var chartbugscountdata = _chartsQueries.GetDeveloperProjectWiseBugsCountbyUserId(user);

            var openbugcount = (from temp in chartbugscountdata
                                select temp.Open).ToList();

            var closebugcount = (from temp in chartbugscountdata
                                 select temp.Closed).ToList();


            var projectnames = (from temp in chartbugscountdata
                                select temp.ProjectName).ToList();


            var listofallcolors = new List<string>()
            {
                "#99ccff",
                "#C9AAFA",
                "#f96987",
                "#ffdc73",
                "#83e7a8",
                "#FDBC84",
                "#9ccc34",
                "#FFDF7D",
                "#ff7474",
                "#ff9936",
                "#C5D3E3",
                "#FFE9D2"
            };

            #endregion

            var pieRoot = new BarChartRoot();
            pieRoot.labels = projectnames;
            pieRoot.datasets = new List<BarChartDataset>()
            {
                new BarChartDataset()
                  {
                      label = "Open",
                      barPercentage = 0,
                      barThickness = 25,
                      maxBarThickness = 15,
                      minBarLength = 2,
                      backgroundColor = "#99ccff",
                      borderColor = "rgba(60,141,188,0.8)",
                      pointColor = "#3b8bba",
                      pointHighlightFill = "#fff",
                      pointHighlightStroke = "rgba(60,141,188,1)",
                      pointRadius = false,
                      pointStrokeColor = "rgba(60,141,188,1)",
                      data = openbugcount

                  },
                  new BarChartDataset()
                  {
                      label = "Closed",
                      barPercentage = 0,
                      barThickness = 25,
                      maxBarThickness = 15,
                      minBarLength = 2,
                      backgroundColor = "#9ccc34",
                      borderColor = "rgba(210, 214, 222, 1)",
                      pointColor = "rgba(210, 214, 222, 1)",
                      pointHighlightFill = "#fff",
                      pointHighlightStroke = "rgba(60,141,188,1)",
                      pointRadius = false,
                      pointStrokeColor = "#c1c7d1",
                      data = closebugcount

                  }
            };
            return Json(pieRoot);
        }

        [AuthorizeDeveloper]
        public IActionResult GetPieOpenCloseChartData(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            #region Priority Pie Chart
            var chartsOpenclosedata = _chartsQueries.GetDevelopersBugsCountPieChartbyUserId(user, requestProject.ProjectId);

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

        [AuthorizeDeveloper]
        public IActionResult GetProjectsWiseCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofbugHistory = _chartsQueries.GetDeveloperProjectWiseBugsCountbyUserId(user, requestProject.ProjectId);
            return PartialView("_chartstables", listofbugHistory);
        }
        [AuthorizeDeveloper]
        public IActionResult GetStatusWiseBugsCountDeveloper(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofStatus = _chartsQueries.GetDevelopersStatusWiseBugCount(user, requestProject.ProjectId);
            return Json(listofStatus);
        }

        [AuthorizeDeveloper]
        public IActionResult GetDeveloperSeveritywiseCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofSeverity = _chartsQueries.GetDevelopersSeveritywiseCount(user, requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Severity Type",
                ReporterCommonViewModel = listofSeverity
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }
        [AuthorizeDeveloper]
        public IActionResult GetBugTypeProjectwiseCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofBugType = _chartsQueries.GetDevelopersBugTypeProjectwiseCount(user, requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "BugType Type",
                ReporterCommonViewModel = listofBugType
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }
        [AuthorizeDeveloper]
        public IActionResult GetTestedEnvironmentProjectwiseCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofTestedEnvironment = _chartsQueries.GetDevelopersTestedEnvironmentProjectwiseCount(user, requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Environment Type",
                ReporterCommonViewModel = listofTestedEnvironment
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }


        [HttpGet]
        [AuthorizeDeveloperLead]
        public IActionResult MyDashboard()
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var developerTeamLead = new DeveloperTeamLeadChartTableViewModel()
            {
                ListofProjects = _projectQueries.GetAssignedProjectListWithoutSelect(user)
            };

            return View(developerTeamLead);
        }

        [AuthorizeDeveloperLead]
        public IActionResult GetPieChartTeamLeadData(RequestProjectReporter requestProject)
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

        [AuthorizeDeveloperLead]
        public IActionResult GetPriorityChartTeamLeadData(RequestProjectReporter requestProject)
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
        [AuthorizeDeveloperLead]
        public IActionResult GetPieOpenCloseChartTeamLeadData(RequestProjectReporter requestProject)
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

        [AuthorizeDeveloperLead]
        public IActionResult GetProjectsWiseTeamLeadCount(RequestProjectReporter requestProject)
        {
            var listofbugHistory = _chartsQueries.GetReporterLeadProjectWiseBugsCountbyUserId(requestProject.ProjectId);
            return PartialView("_chartstables", listofbugHistory);
        }

        [AuthorizeDeveloperLead]
        public IActionResult GetStatusWiseBugsCountLead(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofStatus = _chartsQueries.GetLeadStatusWiseBugCount(user, requestProject.ProjectId);
            return Json(listofStatus);
        }

        [AuthorizeDeveloperLead]
        public IActionResult GetLeadSeveritywiseCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofSeverity = _chartsQueries.GetLeadSeveritywiseCount(requestProject.ProjectId);

            var reporterCommonMainViewModel = new ReporterCommonMainViewModel()
            {
                Heading = "Severity Type",
                ReporterCommonViewModel = listofSeverity
            };

            return PartialView("_CommonTables", reporterCommonMainViewModel);
        }

        [AuthorizeDeveloperLead]
        public IActionResult GetLeadBugTypeProjectwiseCount(RequestProjectReporter requestProject)
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

        [AuthorizeDeveloperLead]
        public IActionResult GetLeadTestedEnvironmentProjectwiseCount(RequestProjectReporter requestProject)
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

        [AuthorizeDeveloperandLead]
        public IActionResult GetDeveloperTeamsBugsCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var dataCount = _chartsQueries.GetDeveloperTeamsBugsCount(requestProject.ProjectId);
            return PartialView("_CommonStatusTables", dataCount);
        }

        [AuthorizeDeveloperLead]
        public IActionResult GetBrowserChartTeamLeadData(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            var browserdata = _chartsQueries.GetBrowserNamesofTestedBugs(requestProject.ProjectId);

            var bugPrioritycount = (from temp in browserdata
                select temp.TotalCount).ToList();

            var prioritylist = (from temp in browserdata
                select temp.TextValue).ToList();


            var listPrioritycolortoshow = new List<string>();
            var listofallcolors = new List<string>()
            {
                "#99ccff",
                "#C9AAFA",
                "#f96987",
                "#ffdc73",
                "#83e7a8",
                "#FDBC84",
                "#9ccc34",
                "#FFDF7D",
                "#ff7474",
                "#ff9936",
                "#C5D3E3",
                "#FFE9D2"
            };

            for (var index = 0; index < browserdata.Count; index++)
            {
                var value = listofallcolors[index];
                listPrioritycolortoshow.Add(value);
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

        public IActionResult GetStarDeveloperCount(RequestProjectReporter requestProject)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var dataCount = _chartsQueries.GetStartDeveloperCount(requestProject.ProjectId);
            return PartialView("_CommonStar", dataCount);
        }
    }
}
