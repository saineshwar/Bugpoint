using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Data.ApplicationLog.Queries;

namespace BugPoint.Application.Areas.Administration.Controllers
{
    [Area("Administration")]
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class ViewLogsController : Controller
    {
        private readonly IApplicationLogQueries _applicationLogQueries;
        private readonly INotificationService _notificationService;
        public ViewLogsController(IApplicationLogQueries applicationLogQueries, INotificationService notificationService)
        {
            _applicationLogQueries = applicationLogQueries;
            _notificationService = notificationService;
        }
        public IActionResult Logs()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllLogs()
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
                var logdata = _applicationLogQueries.ShowAllLogs(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = logdata.Count();
                var data = logdata.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                _notificationService.DangerNotification("Alert","No Error Found");
                return RedirectToAction("GridAllLogs");
            }

            var details = _applicationLogQueries.ErrorDetails(id);
            return View(details);
        }
    }
}
