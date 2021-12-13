using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Common;
using BugPoint.Data.Assigned.Command;
using BugPoint.Data.Assigned.Queries;
using BugPoint.Data.Project.Queries;
using BugPoint.Data.RoleMaster.Queries;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.MovingBugs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using BugPoint.Model.Bugs;
using BugPoint.Data.Bugs.Queries;
using BugPoint.Data.Bugs.Command;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using BugPoint.Services.MailHelper;
using BugPoint.Model.UserMaster;

namespace BugPoint.Application.Areas.Administration.Controllers
{
    [Area("Administration")]
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class MovingBugsController : Controller
    {
        private readonly IProjectQueries _projectQueries;
        private readonly IRoleQueries _roleQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IMovingBugsQueries _movingBugsQueries;
        private readonly IMovingBugsCommand _movingBugsCommand;
        private readonly IBugHistoryHelper _bugHistoryHelper;
        private readonly IBugHistoryCommand _bugHistoryCommand;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailingService _mailingService;

        public MovingBugsController(
            IBugHistoryCommand bugHistoryCommand,
            IMailingService mailingService, 
            IWebHostEnvironment webHostEnvironment,
            IBugHistoryHelper bugHistoryHelper,
            IProjectQueries projectQueries,
            IRoleQueries roleQueries,
            IUserMasterQueries userMasterQueries,
            IMovingBugsQueries movingBugsQueries,
            IMovingBugsCommand movingBugsCommand)
        {
            _mailingService = mailingService;
            _webHostEnvironment = webHostEnvironment;
            _bugHistoryHelper = bugHistoryHelper;
            _projectQueries = projectQueries;
            _roleQueries = roleQueries;
            _userMasterQueries = userMasterQueries;
            _movingBugsQueries = movingBugsQueries;
            _movingBugsCommand = movingBugsCommand;
            _bugHistoryCommand = bugHistoryCommand;
        }

  
        [HttpGet]
        public IActionResult Process()
        {
            var movingBugsViewModel = new MovingBugsViewModel()
            {
                ListofRoles = _roleQueries.ListofDevandTesterLeadsRoles()
            };
            return View(movingBugsViewModel);
        }

        [HttpPost]
        public IActionResult Process(MovingBugsResponse movingBugs)
        {

            if (movingBugs.ListofBugs == null || movingBugs.ListofBugs.Count == 0)
            {
                return Json(new { status = "Validation" });
            }
            else if (movingBugs.ProjectId == null)
            {
                return Json(new { status = "Project_Validation" });
            }
            else if (movingBugs.RoleId == null)
            {
                return Json(new { status = "Role_Validation" });
            }
            else if (movingBugs.FromUserId == null)
            {
                return Json(new { status = "FromUser_Validation" });
            }
            else if (movingBugs.ToUserId == null)
            {
                return Json(new { status = "ToUser_Validation" });
            }
            else
            {
                var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                _movingBugsCommand.MovingBugsProcess(movingBugs, user);


                var fromuser = _userMasterQueries.GetUserDetailsbyUserId(movingBugs.FromUserId);
                var touser = _userMasterQueries.GetUserDetailsbyUserId(movingBugs.ToUserId);

                var fromuserfullname = $"{fromuser.FirstName} {fromuser.LastName}";
                var touserfullname = $"{touser.FirstName} {touser.LastName}";

                foreach (var item in movingBugs.ListofBugs)
                {
                    BugHistoryModel ticketHistory = new BugHistoryModel
                    {
                        UserId = user,
                        Message = _bugHistoryHelper.MovingBugsMessage(fromuserfullname, touserfullname),
                        ProcessDate = DateTime.Now,
                        BugId = item,
                        PriorityId = null,
                        StatusId = null,
                        AssignedTo = movingBugs.ToUserId
                    };

                    _bugHistoryCommand.InsertBugHistory(ticketHistory); 
                }

                AssignedToEmailtoSend(fromuserfullname, touserfullname, touser);

            }
            return Json(new { status = "Success" });
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

        public IActionResult GetToUserUsernames(BugMovementRequestModel bugMovementRequestModel)
        {
            if (!string.IsNullOrEmpty(bugMovementRequestModel.Username) && bugMovementRequestModel.RoleId != null && bugMovementRequestModel.UserId != null)
            {
                var projectlist = _userMasterQueries.ListofUserSpecificUser(bugMovementRequestModel);
                return Json(projectlist);
            }
            else
            {
                return Json(new List<SelectListItem>());
            }
        }


        [HttpPost]
        public IActionResult GridAllAssignedBugs()
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
                var fromUserId = string.IsNullOrEmpty(Request.Form["fromUserId"].FirstOrDefault()) ? "0" : Request.Form["fromUserId"].FirstOrDefault();
                var roleId = string.IsNullOrEmpty(Request.Form["roleId"].FirstOrDefault()) ? "0" : Request.Form["roleId"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var querydata = _movingBugsQueries.ShowAllAssignedBugs(sortColumn, sortColumnDirection, searchValue,
                    Convert.ToInt32(projectId),
                    Convert.ToInt32(fromUserId),
                    Convert.ToInt32(roleId));
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

        private void AssignedToEmailtoSend(string fromuser , string touser , UserMasterModel usermasterModel)
        {
            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "bugreply.html");
                var fullname = HttpContext.Session.GetString(AllSessionKeys.FullName);
                

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var str = new StringBuilder();
                str.Append($"All Bugs of {fromuser} are assigned to you.");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", $" {touser}");

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = usermasterModel.EmailId,
                    Body = mailText,
                    Subject = $"[Bulk Action] All bugs of {fromuser} are assigned to you."
                };

                _mailingService.SendEmailAsync(sendingMailRequest);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
