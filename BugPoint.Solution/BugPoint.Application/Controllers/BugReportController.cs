using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.Project.Queries;
using BugPoint.Data.Reports.Queries;
using BugPoint.ViewModel.Charts;
using BugPoint.ViewModel.Reports;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace BugPoint.Application.Controllers
{
    public class BugReportController : Controller
    {
        private readonly IProjectQueries _projectQueries;
        private readonly IReportQueries _reportQueries;
        private readonly INotificationService _notificationService;
        public BugReportController(IProjectQueries projectQueries, IReportQueries reportQueries, INotificationService notificationService)
        {
            _projectQueries = projectQueries;
            _reportQueries = reportQueries;
            _notificationService = notificationService;
        }

        [HttpGet]
        [AuthorizeDeveloperLead]
        public IActionResult DeveloperReport()
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var exportReportViewModel = new ExportReportViewModel()
            {
                ListofProjects = _projectQueries.GetAssignedProjectList(user),
                ListofReportType = _reportQueries.ReportTypeList()
            };

            return View(exportReportViewModel);
        }


        [HttpPost]
        public IActionResult DeveloperReport(ExportReportViewModel exportReportViewModel)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            exportReportViewModel.ListofProjects = _projectQueries.GetAssignedProjectList(user);
            exportReportViewModel.ListofReportType = _reportQueries.ReportTypeList();

            if (exportReportViewModel.ReportsId == null)
            {
                _notificationService.DangerNotification("Message", "Report Type is Required");
                return View(exportReportViewModel);
            }

            if (exportReportViewModel.ReportsId == 1)
            {
                string reportname = $"Project_Developer_Wise_{Guid.NewGuid():N}.xlsx";
                var list = _reportQueries.GetDeveloperTeamsProjectwiseReport(exportReportViewModel.ProjectId);
                if (list.Count > 0)
                {
                    var exportbytes = ExporttoExcel<BugsReportCountViewModel>(list, reportname);
                    return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                }
                else
                {
                    _notificationService.DangerNotification("Message", "No Data to Export");
                    return View(exportReportViewModel);
                }

            }
            else if (exportReportViewModel.ReportsId == 2)
            {
                string reportname = $"Project_Component_Wise_{Guid.NewGuid():N}.xlsx";
                var list = _reportQueries.GetDeveloperTeamsProjectwiseComponentReport(exportReportViewModel.ProjectId);
                if (list.Count > 0)
                {
                    var exportbytes = ExporttoExcel<BugsReportComponentWiseCountViewModel>(list, reportname);
                    return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                }
                else
                {
                    _notificationService.DangerNotification("Message", "No Data to Export");
                    return View(exportReportViewModel);
                }
            }
            else if (exportReportViewModel.ReportsId == 3)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Fromdate))
                {
                    _notificationService.DangerNotification("Message", "Select From Date");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Todate))
                {
                    _notificationService.DangerNotification("Message", "Select To Date");
                }
                else
                {
                    string reportname = $"Project_Open_Close_Detail_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetDeveloperBugOpenCloseDetailsReport(exportReportViewModel.ProjectId, exportReportViewModel.Fromdate, exportReportViewModel.Todate);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugReportDetailsExport>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }
            else if (exportReportViewModel.ReportsId == 4)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Fromdate))
                {
                    _notificationService.DangerNotification("Message", "Select From Date");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Todate))
                {
                    _notificationService.DangerNotification("Message", "Select To Date");
                }
                else
                {
                    string reportname = $"Bug_Detail_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetBugDetailsbyCreatedDateReport(exportReportViewModel.ProjectId, exportReportViewModel.Fromdate, exportReportViewModel.Todate);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugDetailViewReportModel>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }
            else if (exportReportViewModel.ReportsId == 5)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else
                {
                    string reportname = $"Bug_TimeTakenReport_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetTimeTakeReport(exportReportViewModel.ProjectId);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugTimeTakenReportExport>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }

            return View(exportReportViewModel);
        }

        [HttpGet]
        [AuthorizeTesterLeadAttribute]
        public IActionResult TesterReport()
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var exportReportViewModel = new ExportReportViewModel()
            {
                ListofProjects = _projectQueries.GetAssignedProjectList(user),
                ListofReportType = _reportQueries.ReportTypeList()
            };

            return View(exportReportViewModel);
        }


        [HttpPost]
        public IActionResult TesterReport(ExportReportViewModel exportReportViewModel)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            exportReportViewModel.ListofProjects = _projectQueries.GetAssignedProjectList(user);
            exportReportViewModel.ListofReportType = _reportQueries.ReportTypeList();

            if (exportReportViewModel.ReportsId == null)
            {
                _notificationService.DangerNotification("Message", "Report Type is Required");
                return View(exportReportViewModel);
            }

            if (exportReportViewModel.ReportsId == 1)
            {
                string reportname = $"Project_Tester_Wise_{Guid.NewGuid():N}.xlsx";
                var list = _reportQueries.GetTesterTeamsProjectwiseReport(exportReportViewModel.ProjectId);
                if (list.Count > 0)
                {
                    var exportbytes = ExporttoExcel<BugsReportTesterCountViewModel>(list, reportname);
                    return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                }
                else
                {
                    _notificationService.DangerNotification("Message", "No Data to Export");
                    return View(exportReportViewModel);
                }

            }
            else if (exportReportViewModel.ReportsId == 2)
            {
                string reportname = $"Project_Component_Wise_{Guid.NewGuid():N}.xlsx";
                var list = _reportQueries.GetTesterTeamsProjectwiseComponentReport(exportReportViewModel.ProjectId);
                if (list.Count > 0)
                {
                    var exportbytes = ExporttoExcel<BugsReportComponentWiseCountViewModel>(list, reportname);
                    return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                }
                else
                {
                    _notificationService.DangerNotification("Message", "No Data to Export");
                    return View(exportReportViewModel);
                }
            }
            else if (exportReportViewModel.ReportsId == 3)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Fromdate))
                {
                    _notificationService.DangerNotification("Message", "Select From Date");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Todate))
                {
                    _notificationService.DangerNotification("Message", "Select To Date");
                }
                else
                {
                    string reportname = $"Project_Open_Close_Detail_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetTesterBugOpenCloseDetailsReport(exportReportViewModel.ProjectId, exportReportViewModel.Fromdate, exportReportViewModel.Todate);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugReportDetailsExport>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }
            else if (exportReportViewModel.ReportsId == 4)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Fromdate))
                {
                    _notificationService.DangerNotification("Message", "Select From Date");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Todate))
                {
                    _notificationService.DangerNotification("Message", "Select To Date");
                }
                else
                {
                    string reportname = $"Bug_Detail_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetBugDetailsbyCreatedDateReport(exportReportViewModel.ProjectId, exportReportViewModel.Fromdate, exportReportViewModel.Todate);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugDetailViewReportModel>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }
            else if (exportReportViewModel.ReportsId == 5)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else
                {
                    string reportname = $"Bug_TimeTakenReport_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetTimeTakeReport(exportReportViewModel.ProjectId);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugTimeTakenReportExport>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }

            return View(exportReportViewModel);
        }


        [HttpGet]
        [AuthorizeProjectManagerAttribute]
        public IActionResult ManagerReport()
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var exportReportViewModel = new ExportReportManagerViewModel()
            {
                ListofProjects = _projectQueries.GetAllProjectList(),
                ListofReportType = _reportQueries.ReportTypeList(),
                TypeofRole = _reportQueries.RoleTypeList()
            };

            return View(exportReportViewModel);
        }

        [HttpPost]
        public IActionResult ManagerReport(ExportReportManagerViewModel exportReportViewModel)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            exportReportViewModel.ListofProjects = _projectQueries.GetAssignedProjectList(user);
            exportReportViewModel.ListofReportType = _reportQueries.ReportTypeList();

            if (exportReportViewModel.ReportsId == null)
            {
                _notificationService.DangerNotification("Message", "Report Type is Required");
                return View(exportReportViewModel);
            }

            //Developer
            if (exportReportViewModel.ReportsId == 1 && exportReportViewModel.RoleId == 1)
            {
                string reportname = $"Project_Developer_Wise_{Guid.NewGuid():N}.xlsx";
                var list = _reportQueries.GetDeveloperTeamsProjectwiseReport(exportReportViewModel.ProjectId);
                if (list.Count > 0)
                {
                    var exportbytes = ExporttoExcel<BugsReportCountViewModel>(list, reportname);
                    return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                }
                else
                {
                    _notificationService.DangerNotification("Message", "No Data to Export");
                    return View(exportReportViewModel);
                }

            }
            // Tester
            else if (exportReportViewModel.ReportsId == 1 && exportReportViewModel.RoleId == 2)
            {
                string reportname = $"Project_Tester_Wise_{Guid.NewGuid():N}.xlsx";
                var list = _reportQueries.GetTesterTeamsProjectwiseReport(exportReportViewModel.ProjectId);
                if (list.Count > 0)
                {
                    var exportbytes = ExporttoExcel<BugsReportTesterCountViewModel>(list, reportname);
                    return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                }
                else
                {
                    _notificationService.DangerNotification("Message", "No Data to Export");
                    return View(exportReportViewModel);
                }
            }
            else if (exportReportViewModel.ReportsId == 2 && exportReportViewModel.RoleId == 1)
            {
                string reportname = $"Project_Component_Wise_{Guid.NewGuid():N}.xlsx";
                var list = _reportQueries.GetDeveloperTeamsProjectwiseComponentReport(exportReportViewModel.ProjectId);
                if (list.Count > 0)
                {
                    var exportbytes = ExporttoExcel<BugsReportComponentWiseCountViewModel>(list, reportname);
                    return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                }
                else
                {
                    _notificationService.DangerNotification("Message", "No Data to Export");
                    return View(exportReportViewModel);
                }
            }

            else if (exportReportViewModel.ReportsId == 2 && exportReportViewModel.RoleId == 2)
            {
                string reportname = $"Project_Component_Wise_{Guid.NewGuid():N}.xlsx";
                var list = _reportQueries.GetTesterTeamsProjectwiseComponentReport(exportReportViewModel.ProjectId);
                if (list.Count > 0)
                {
                    var exportbytes = ExporttoExcel<BugsReportComponentWiseCountViewModel>(list, reportname);
                    return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                }
                else
                {
                    _notificationService.DangerNotification("Message", "No Data to Export");
                    return View(exportReportViewModel);
                }
            }
            else if (exportReportViewModel.ReportsId == 3 && exportReportViewModel.RoleId == 1)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Fromdate))
                {
                    _notificationService.DangerNotification("Message", "Select From Date");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Todate))
                {
                    _notificationService.DangerNotification("Message", "Select To Date");
                }
                else
                {
                    string reportname = $"Project_Open_Close_Detail_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetDeveloperBugOpenCloseDetailsReport(exportReportViewModel.ProjectId, exportReportViewModel.Fromdate, exportReportViewModel.Todate);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugReportDetailsExport>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }

            else if (exportReportViewModel.ReportsId == 3 && exportReportViewModel.RoleId == 2)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Fromdate))
                {
                    _notificationService.DangerNotification("Message", "Select From Date");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Todate))
                {
                    _notificationService.DangerNotification("Message", "Select To Date");
                }
                else
                {
                    string reportname = $"Project_Open_Close_Detail_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetTesterBugOpenCloseDetailsReport(exportReportViewModel.ProjectId, exportReportViewModel.Fromdate, exportReportViewModel.Todate);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugReportDetailsExport>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }
            else if (exportReportViewModel.ReportsId == 4)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Fromdate))
                {
                    _notificationService.DangerNotification("Message", "Select From Date");
                }
                else if (string.IsNullOrEmpty(exportReportViewModel.Todate))
                {
                    _notificationService.DangerNotification("Message", "Select To Date");
                }
                else
                {
                    string reportname = $"Bug_Detail_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetBugDetailsbyCreatedDateReport(exportReportViewModel.ProjectId, exportReportViewModel.Fromdate, exportReportViewModel.Todate);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugDetailViewReportModel>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }
            else if (exportReportViewModel.ReportsId == 5)
            {
                if (exportReportViewModel.ProjectId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Project");
                }
                else if (exportReportViewModel.ReportsId == null)
                {
                    _notificationService.DangerNotification("Message", "Select Report Type");
                }
                else
                {
                    string reportname = $"Bug_TimeTakenReport_{Guid.NewGuid():N}.xlsx";
                    var list = _reportQueries.GetTimeTakeReport(exportReportViewModel.ProjectId);
                    if (list.Count > 0)
                    {
                        var exportbytes = ExporttoExcel<BugTimeTakenReportExport>(list, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "No Data to Export");
                        return View(exportReportViewModel);
                    }
                }
            }

            return View(exportReportViewModel);
        }


        private byte[] ExporttoExcel<T>(List<T> table, string filename)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Light1);
            return pack.GetAsByteArray();
        }
    }
}
