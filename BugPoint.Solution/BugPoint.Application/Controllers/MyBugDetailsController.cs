using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.Assigned.Queries;
using BugPoint.Data.Bugs.Command;
using BugPoint.Data.Bugs.Queries;
using BugPoint.Data.Masters.Queries;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.Model.Bugs;
using BugPoint.Model.UserMaster;
using BugPoint.Services.AwsHelper;
using BugPoint.Services.MailHelper;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.BugsList;
using BugPoint.ViewModel.UserMaster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    [AuthorizeDeveloperandLead]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class MyBugDetailsController : Controller
    {
        private readonly IBugQueries _bugQueries;
        private readonly IBugCommand _bugCommand;
        private readonly INotificationService _notificationService;
        private readonly IMastersQueries _mastersQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IBugHistoryCommand _bugHistoryCommand;
        private readonly IBugHistoryHelper _bugHistoryHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailingService _mailingService;
        private readonly IAssignProjectQueries _assignProjectQueries;
        private readonly IAwsS3HelperService _awsS3HelperService;
        private readonly AwsSettings _awsSettings;
        private readonly AppSettingsProperties _appsettings;

        public MyBugDetailsController(IBugQueries bugQueries,
            IBugCommand bugCommand,
            INotificationService notificationService,
            IMastersQueries mastersQueries,
            IUserMasterQueries userMasterQueries,
            IBugHistoryCommand bugHistoryCommand,
            IBugHistoryHelper bugHistoryHelper,
            IWebHostEnvironment webHostEnvironment,
            IMailingService mailingService,
            IAssignProjectQueries assignProjectQueries,
            IAwsS3HelperService awsS3HelperService,
            IOptions<AwsSettings> awsSettings,
            IOptions<AppSettingsProperties> appsettings)
        {
            _bugQueries = bugQueries;
            _bugCommand = bugCommand;
            _notificationService = notificationService;
            _mastersQueries = mastersQueries;
            _userMasterQueries = userMasterQueries;
            _bugHistoryCommand = bugHistoryCommand;
            _bugHistoryHelper = bugHistoryHelper;
            _webHostEnvironment = webHostEnvironment;
            _mailingService = mailingService;
            _assignProjectQueries = assignProjectQueries;
            _awsS3HelperService = awsS3HelperService;
            _awsSettings = awsSettings.Value;
            _appsettings = appsettings.Value;
        }

        [HttpGet]
        [AuthenticateDeveloperAccess]
        public IActionResult Details(long? id)
        {
            var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var bugDetailViewModel = _bugQueries.GetBugDetailsbyBugId(id);

            if (bugDetailViewModel == null)
            {
                _notificationService.DangerNotification("Message", "You are trying to access bug which is not there!");

                if (currentrole == Convert.ToInt32(RolesHelper.Roles.Developer))
                {
                    return RedirectToAction("Show", "MyBugList");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.DeveloperTeamLead))
                {
                    return RedirectToAction("AllReportedBugs", "MyBugList");
                }
            }

            if (!_assignProjectQueries.CheckProjectAlreadyAssigned(bugDetailViewModel.ProjectId, user))
            {
                _notificationService.DangerNotification("Message", "You do have Permission to view this Bug Details!");

                if (currentrole == Convert.ToInt32(RolesHelper.Roles.Developer))
                {
                    return RedirectToAction("Show", "MyBugList");
                }
                else if (currentrole == Convert.ToInt32(RolesHelper.Roles.DeveloperTeamLead))
                {
                    return RedirectToAction("AllReportedBugs", "MyBugList");
                }
            }

            ViewBag.CurrentBugStatus = bugDetailViewModel.StatusId;
            ViewBag.Currentrole = currentrole;

            var displayBugViewModel = new DisplayBugUserViewModel()
            {
                BugDetailViewModel = bugDetailViewModel,
                ListofAttachments = _bugQueries.GetListAttachmentsBybugId(id),
                BugReplyViewModel = new BugReplyUserViewModel()
                {
                    BugId = bugDetailViewModel.BugId,
                    ListofStatus = _mastersQueries.ListofUserStatus(),
                    ListofResolutions = _mastersQueries.ListofResolution()
                },
                ViewBugReplyMainModel = new ViewBugReplyMainModel()
                {
                    ListofTicketreply = _bugQueries.ListofHistoryTicketReplies(id),
                    ListofReplyAttachment = new List<ReplyAttachmentModel>()
                },
                ExpressChangesViewModel = new ExpressChangesUserViewModel()
            };


            return View(displayBugViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Details(BugReplyUserViewModel bugReplyViewModel)
        {
            try
            {
                var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                string currentdatetime = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture);

                var bugReplyModel = new BugReplyModel()
                {
                    BugId = bugReplyViewModel.BugId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = user,
                    CreatedDateDisplay = currentdatetime,
                    BugReplyId = 0,
                    DeleteStatus = false,
                };

                var bugReplyDetailsModel = new BugReplyDetailsModel()
                {
                    BugId = bugReplyViewModel.BugId,
                    Description = bugReplyViewModel.Description,
                    BugReplyId = 0,
                };

                var listofattachments = new List<BugAttachmentsViewModel>();
                var files = HttpContext.Request.Form.Files;
                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            //Getting FileName

                            var fileName = Path.GetFileName(file.FileName);
                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid().ToString("N"));
                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);
                            // concatenating  FileName + FileExtension
                            var newFileName = String.Concat(myUniqueFileName, fileExtension);


                            await using var target = new MemoryStream();
                            await file.CopyToAsync(target);

                            var attachments = new BugAttachmentsViewModel();
                            attachments.BugId = bugReplyViewModel.BugId;
                            attachments.CreatedBy = user;
                            attachments.GenerateAttachmentName = newFileName;
                            attachments.OriginalAttachmentName = file.FileName;
                            attachments.AttachmentType = fileExtension;
                            attachments.CreatedOn = DateTime.Now;
                            if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                            {
                                attachments.AttachmentBase64 = Convert.ToBase64String(target.ToArray());
                            }
                            else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                            {
                                attachments.AttachmentBase64 = null;
                                attachments.BucketName = _awsSettings.BucketName;
                                attachments.DirectoryName = "documents";
                            }
                            listofattachments.Add(attachments);

                            if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                            {
                                bool status = await _awsS3HelperService.UploadFileAsync(target, newFileName, "documents");
                            }

                          
                        }
                    }
                }

                var bugtracking = _bugQueries.GetBugTrackingbybugId(bugReplyViewModel.BugId);
                bugtracking.StatusId = bugReplyViewModel.StatusId;
                bugtracking.ModifiedOn = DateTime.Now;

                if (bugReplyViewModel.ResolutionId != null)
                {
                    bugtracking.ResolutionId = bugReplyViewModel.ResolutionId;
                  
                    bugtracking.ModifiedBy = user;
                }

                var result = _bugCommand.AddReply(bugReplyModel, bugReplyDetailsModel, listofattachments, bugtracking);

                if (result)
                {
                    string statusname = _mastersQueries.GetStatusBystatusId(Convert.ToInt32(bugReplyViewModel.StatusId));
                    string resolutionname = _mastersQueries.GetResolutionByResolutionId(bugReplyViewModel.ResolutionId);
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);

                    string replymessage;

                    if (bugReplyViewModel.ResolutionId != null)
                    {
                        replymessage = _bugHistoryHelper.ReplyMessage(fullName, bugReplyViewModel.BugId, statusname, resolutionname);
                    }
                    else
                    {
                        replymessage = _bugHistoryHelper.ReplyMessage(fullName, bugReplyViewModel.BugId, statusname);
                    }

                    var bugHistory = new BugHistoryModel();
                    bugHistory.UserId = user;
                    bugHistory.Message = replymessage;
                    bugHistory.ProcessDate = DateTime.Now;
                    bugHistory.BugId = bugReplyViewModel.BugId;
                    bugHistory.PriorityId = null;
                    bugHistory.StatusId = null;
                    bugHistory.AssignedTo = null;
                    bugHistory.ClosedOn = null;

                    _bugHistoryCommand.InsertBugHistory(bugHistory);

                    _notificationService.SuccessNotification("Message", "Replied on Bug Successfully!");

                    ReplyEmailtoSend(bugReplyViewModel.BugId, statusname, bugReplyViewModel.Description, fullName);
                }

                return Json(new { Result = "success", Message = "Replied on Bug Successfully!" });
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Something Went Wrong Please try again!" });
                throw;
            }
        }


        public JsonResult ChangePriority(ChangePriorityRequestModel changePriorityRequestModel)
        {
            try
            {
                var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                var result = _bugCommand.ChangeBugPriority(changePriorityRequestModel);
                if (result)
                {

                    BugHistoryModel ticketHistory = new BugHistoryModel
                    {
                        UserId = user,
                        Message = _bugHistoryHelper.ChangePriorityMessage(changePriorityRequestModel.PriorityId),
                        ProcessDate = DateTime.Now,
                        BugId = changePriorityRequestModel.BugId,
                        PriorityId = changePriorityRequestModel.PriorityId,
                        StatusId = null,
                        AssignedTo = null
                    };
                    _bugHistoryCommand.InsertBugHistory(ticketHistory);


                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult ChangeAssignedTo(ChangeAssignedUserRequestModel changeAssignedUser)
        {
            try
            {
                var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                var fullname = HttpContext.Session.GetString(AllSessionKeys.FullName);
                var result = _bugCommand.ChangeBugAssignedUser(changeAssignedUser);
                if (result)
                {
                    var userMaster = _userMasterQueries.GetUserDetailsbyUserId(changeAssignedUser.UserId);

                    var ticketHistory = new BugHistoryModel
                    {
                        UserId = user,
                        Message = $"Bug {changeAssignedUser.BugId} is Assigned to {userMaster.UserName} by {fullname}",
                        ProcessDate = DateTime.Now,
                        BugId = changeAssignedUser.BugId,
                        PriorityId = null,
                        StatusId = null,
                        AssignedTo = changeAssignedUser.UserId
                    };
                    _bugHistoryCommand.InsertBugHistory(ticketHistory);

                    AssignedToEmailtoSend(changeAssignedUser.BugId, fullname, userMaster);

                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> DownloadAttachment(long bugId, long attachmentId)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(attachmentId)))
                {
                    var document = _bugQueries.GetAttachmentsBybugId(bugId, attachmentId);
                    if (document != null)
                    {
                        if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                        {
                            var documentdetail =
                                _bugQueries.GetAttachmentDetailsByAttachmentId(bugId, document.AttachmentId);
                            byte[] bytes = System.Convert.FromBase64String(documentdetail.AttachmentBase64);
                            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, document.GenerateAttachmentName);
                        }
                        else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                        {
                            var response = await _awsS3HelperService.ReadFileAsync(document.GenerateAttachmentName, "documents");
                            return File(response.FileStream, response.ContentType, document.GenerateAttachmentName);
                        }
                    }

                    return RedirectToAction("Show", "MyBugList");
                }
                else
                {
                    return RedirectToAction("Show", "MyBugList");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> DownloadReplyAttachment(long bugId, long attachmentId)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(attachmentId)))
                {
                    var document = _bugQueries.GetReplyAttachmentsBybugId(bugId, attachmentId);
                    if (document != null)
                    {
                        if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                        {
                            var documentdetail = _bugQueries.GetReplyAttachmentDetailsByAttachmentId(bugId, document.ReplyAttachmentId);
                            byte[] bytes = System.Convert.FromBase64String(documentdetail.AttachmentBase64);
                            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, document.GenerateAttachmentName);
                        }
                        else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                        {
                            var response = await _awsS3HelperService.ReadFileAsync(document.GenerateAttachmentName, "documents");
                            return File(response.FileStream, response.ContentType, document.GenerateAttachmentName);
                        }
                    }

                    return RedirectToAction("Show", "MyBugList");
                }
                else
                {
                    return RedirectToAction("Show", "MyBugList");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> DeleteAttachment(RequestAttachments requestAttachments)
        {
            try
            {
                var document = _bugQueries.GetAttachmentsBybugId(requestAttachments.BugId, requestAttachments.AttachmentsId);
                var documentdetail = _bugQueries.GetAttachmentDetailsByAttachmentId(requestAttachments.BugId, requestAttachments.AttachmentsId);

                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                {
                    var status =
                        await _awsS3HelperService.RemoveFileAsync(document.GenerateAttachmentName, "documents");
                }

                var result = _bugCommand.DeleteAttachmentByAttachmentId(document, documentdetail);
                if (result)
                {
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

                    var bugHistory = new BugHistoryModel
                    {
                        UserId = user,
                        Message = _bugHistoryHelper.DeleteAttachmentMessage(fullName, requestAttachments.BugId),
                        ProcessDate = DateTime.Now,
                        BugId = requestAttachments.BugId,
                        PriorityId = null,
                        StatusId = null,
                        AssignedTo = null
                    };

                    _bugHistoryCommand.InsertBugHistory(bugHistory);

                    return Json(new { Status = true });
                }
                else
                {
                    return Json(new { Status = false });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> DeleteReplyAttachment(RequestAttachments requestAttachments)
        {
            try
            {
                var document = _bugQueries.GetReplyAttachmentsBybugId(requestAttachments.BugId, requestAttachments.AttachmentsId);
                var documentdetail = _bugQueries.GetReplyAttachmentDetailsByAttachmentId(requestAttachments.BugId, requestAttachments.AttachmentsId);

                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                {
                    var status =
                        await _awsS3HelperService.RemoveFileAsync(document.GenerateAttachmentName, "documents");
                }

                var result = _bugCommand.DeleteReplyAttachmentByAttachmentId(document, documentdetail);
                if (result)
                {
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

                    var bugHistory = new BugHistoryModel
                    {
                        UserId = user,
                        Message = _bugHistoryHelper.DeleteReplyAttachmentMessage(fullName, requestAttachments.BugId),
                        ProcessDate = DateTime.Now,
                        BugId = requestAttachments.BugId,
                        PriorityId = null,
                        StatusId = null,
                        AssignedTo = null
                    };

                    _bugHistoryCommand.InsertBugHistory(bugHistory);

                    return Json(new { Status = true });
                }
                else
                {
                    return Json(new { Status = false });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ChangeStatus(ChangeStatusRequestModel changeStatus)
        {
            try
            {
                var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                var result = _bugCommand.UpdatebugStatus(changeStatus.BugId, (int)StatusHelper.Status.RE_OPENED);
                if (result)
                {

                    var ticketHistory = new BugHistoryModel
                    {
                        UserId = user,
                        Message = _bugHistoryHelper.ChangeStatusMessage((int)StatusHelper.Status.RE_OPENED),
                        ProcessDate = DateTime.Now,
                        BugId = changeStatus.BugId,
                        PriorityId = null,
                        StatusId = (int)StatusHelper.Status.RE_OPENED,
                        AssignedTo = null
                    };
                    _bugHistoryCommand.InsertBugHistory(ticketHistory);


                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult GetDeveloperList(ChangeAssignedDeveloperRequestModel changeAssigned)
        {
            if (!string.IsNullOrEmpty(changeAssigned.Username) && changeAssigned.ProjectId != null)
            {
                var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                var userlist = _assignProjectQueries.GetDevelopersandTeamLeadAssignedtoProjectNotSelf(changeAssigned.ProjectId, user, changeAssigned.Username);
                return Json(userlist);
            }
            else
            {
                return Json(new List<RequestDevelopers>());
            }
        }

        private void ReplyEmailtoSend(long? bugId, string statusname, string description, string repliedby)
        {
            try
            {

                string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/BugDetails/Details/{bugId}";

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "bugreply.html");
                var recipientdetails = _bugQueries.GetBugTesterbybugId(bugId);

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var str = new StringBuilder();
                str.Append("<table style='border-collapse: collapse; border:1px solid #ddd; font-size:14px;' class='one-column' cellpadding='0'  cellspacing='0' width='100%'>  <tbody>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd; '>Bug Id</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{url}'>{bugId}</a> </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Status</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{statusname}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Description</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{description}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Replied by</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{repliedby}</td>");
                str.Append("</tr>");
                str.Append("</tbody></table>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", recipientdetails.RecipientFullName);

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = recipientdetails.RecipientEmailId,
                    Body = mailText,
                    Subject = $"[Bug {bugId}] [Replied] by {repliedby}"
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AssignedToEmailtoSend(long? bugId, string repliedby, UserMasterModel userMasterModel)
        {
            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "bugreply.html");
                var fullname = HttpContext.Session.GetString(AllSessionKeys.FullName);
                string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/MyBugDetails/Details/{bugId}";

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var str = new StringBuilder();
                str.Append("<table style='border-collapse: collapse; border:1px solid #ddd; font-size:14px;' class='one-column' cellpadding='0'  cellspacing='0' width='100%'>  <tbody>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd; '>Bug Id</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{url}'>{bugId}</a> </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Assigned To</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{userMasterModel.FirstName} {userMasterModel.LastName}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Reassigned by</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{repliedby}</td>");
                str.Append("</tr>");
                str.Append("</tbody></table>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", $" {userMasterModel.FirstName} {userMasterModel.LastName}");

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = userMasterModel.EmailId,
                    Body = mailText,
                    Subject = $"[Bug {bugId}] [Assigned to You] by {repliedby}"
                };

                _mailingService.SendEmailAsync(sendingMailRequest);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult GetBugActivities(RequestBugReopen requestBug)
        {
            var listofbugHistory = _bugQueries.GetBugHistorybyBugId(requestBug.BugId);
            return PartialView("_BugActivities", listofbugHistory);
        }
    }
}
