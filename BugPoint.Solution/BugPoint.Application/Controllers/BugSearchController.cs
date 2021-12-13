using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.Bugs.Queries;
using BugPoint.Model.Bugs;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.BugsList;
using BugPoint.ViewModel.Charts;
using Microsoft.AspNetCore.Http;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class BugSearchController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IBugQueries _bugQueries;
        public BugSearchController(INotificationService notificationService, IBugQueries bugQueries)
        {
            _notificationService = notificationService;
            _bugQueries = bugQueries;
        }
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(BugSearchViewModel bugSearch)
        {
            if (!ModelState.IsValid)
            {
                _notificationService.DangerNotification("Message", "You are trying to access bug which is not there!");
                var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);

                if (currentrole == Convert.ToInt32(RolesHelper.Roles.Developer))
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.DeveloperTeamLead))
                {
                    return RedirectToAction("MyDashboard", "Dashboard");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.Tester))
                {
                    return RedirectToAction("Dashboard", "ReporterDashboard");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.TesterTeamLead))
                {
                    return RedirectToAction("MyDashboard", "ReporterDashboard");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.ProjectManager))
                {
                    return RedirectToAction("Dashboard", "Manager");
                }
            }

            return RedirectToAction("BugDetails", "BugSearch", new { id = bugSearch.BugIdSearch });
        }

        public IActionResult BugDetails(long? id)
        {
            var bugDetailViewModel = _bugQueries.GetBugDetailsbyBugId(id);

            if (bugDetailViewModel == null)
            {
                _notificationService.DangerNotification("Message", "You are trying to access bug which is not there!");
                var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);

                if (currentrole == Convert.ToInt32(RolesHelper.Roles.Developer))
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.DeveloperTeamLead))
                {
                    return RedirectToAction("MyDashboard", "Dashboard");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.Tester))
                {
                    return RedirectToAction("Dashboard", "ReporterDashboard");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.TesterTeamLead))
                {
                    return RedirectToAction("MyDashboard", "ReporterDashboard");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.ProjectManager))
                {
                    return RedirectToAction("Dashboard", "Manager");
                }
            }


            var displayBugViewModel = new DisplayBugViewModel()
            {
                BugDetailViewModel = bugDetailViewModel,
                ListofAttachments = _bugQueries.GetListAttachmentsBybugId(id),
                BugReplyViewModel = new BugReplyViewModel()
                {
                    BugId = bugDetailViewModel.BugId,
                    ListofStatus = null
                },
                ViewBugReplyMainModel = new ViewBugReplyMainModel()
                {
                    ListofTicketreply = _bugQueries.ListofHistoryTicketReplies(id),
                    ListofReplyAttachment = new List<ReplyAttachmentModel>()
                },
                ExpressChangesViewModel = new ExpressChangesViewModel()
                {
                    ListofPriority = null,
                    PriorityId = bugDetailViewModel.PriorityId,
                    AssignedToId = bugDetailViewModel.AssignedToId,
                    ListofUsers = null,
                    AssignedTo = bugDetailViewModel.AssignedTo,
                    Priority = bugDetailViewModel.Priority
                }
            };

            return View(displayBugViewModel);
        }
    }
}
