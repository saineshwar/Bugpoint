using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.Bugs.Command;
using BugPoint.Data.Bugs.Queries;
using BugPoint.Model.Bugs;
using BugPoint.Services.AwsHelper;
using BugPoint.Services.MailHelper;
using BugPoint.ViewModel.Bugs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    [AuthorizeDeveloperandLead]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class EditBugController : Controller
    {
        private readonly IBugQueries _bugQueries;
        private readonly INotificationService _notificationService;
        private readonly AwsSettings _awsSettings;
        private readonly AppSettingsProperties _appsettings;
        private readonly IAwsS3HelperService _awsS3HelperService;
        private readonly IBugCommand _bugCommand;
        private readonly IBugHistoryCommand _bugHistoryCommand;
        private readonly IBugHistoryHelper _bugHistoryHelper;
        private readonly IMailingService _mailingService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditBugController(IBugQueries bugQueries,
            INotificationService notificationService,
            IOptions<AwsSettings> awsSettings,
            IOptions<AppSettingsProperties> appsettings,
            IAwsS3HelperService awsS3HelperService,
            IBugCommand bugCommand,
            IBugHistoryCommand bugHistoryCommand,
            IBugHistoryHelper bugHistoryHelper,
            IMailingService mailingService, IWebHostEnvironment webHostEnvironment)
        {
            _bugQueries = bugQueries;
            _notificationService = notificationService;
            _awsS3HelperService = awsS3HelperService;
            _bugCommand = bugCommand;
            _bugHistoryCommand = bugHistoryCommand;
            _bugHistoryHelper = bugHistoryHelper;
            _mailingService = mailingService;
            _webHostEnvironment = webHostEnvironment;
            _awsSettings = awsSettings.Value;
            _appsettings = appsettings.Value;
        }

        [HttpGet]
        public ActionResult EditReply(int? id, int? rid)
        {
            try
            {
                var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
                ViewBag.Currentrole = currentrole;
                var ticketsViewModel = _bugQueries.GetBugReplyEditDetailsbyBugId(id, rid);
                ticketsViewModel.ListofAttachments = _bugQueries.GetListReplyAttachmentsByAttachmentId(id, rid);
                return View(ticketsViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditReply(EditBugReplyViewModel editBugReply)
        {
            try
            {
                var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                if (ModelState.IsValid)
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Any() &&
                        (_bugQueries.ReplyAttachmentsExistbybugId(editBugReply.BugId, editBugReply.BugReplyId)))
                    {
                        _notificationService.DangerNotification("Message",
                            "Delete all pervious uploaded attachments for uploading new attachments");

                        return RedirectToAction("EditReply", "EditBug", new { id = editBugReply.BugId, rid = editBugReply.BugReplyId });
                    }

                    var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                    var bugReplyDetailsViewModel =
                        _bugQueries.GetBugReplyDetailsbyBugId(editBugReply.BugId, editBugReply.BugReplyId);

                    var bugreplyViewModel = _bugQueries.GetBugReplybyBugId(editBugReply.BugId, editBugReply.BugReplyId);
                    bugreplyViewModel.ModifiedOn = DateTime.Now;
                    bugreplyViewModel.ModifiedBy = user;
                    bugReplyDetailsViewModel.Description = WebUtility.HtmlDecode(editBugReply.Description);

                    var listofattachments = new List<BugAttachmentsViewModel>();

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
                                attachments.BugId = editBugReply.BugId;
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

                    var result = _bugCommand.UpdateReply(bugreplyViewModel, bugReplyDetailsViewModel, listofattachments);


                    if (result)
                    {


                        var bugHistory = new BugHistoryModel
                        {
                            UserId = user,
                            Message = _bugHistoryHelper.EditBugReplyMessage(fullName, editBugReply.BugId),
                            ProcessDate = DateTime.Now,
                            BugId = editBugReply.BugId,
                            PriorityId = null,
                            StatusId = null,
                            AssignedTo = null
                        };

                        _bugHistoryCommand.InsertBugHistory(bugHistory);

                        _notificationService.SuccessNotification("Message", $"Bug Reply Details Updated Successfully.");
                        EditReplyEmailtoSend(editBugReply.BugId, WebUtility.HtmlDecode(editBugReply.Description), fullName);
                        return RedirectToAction("Details", "MyBugDetails", new { id = editBugReply.BugId });
                    }
                }

                var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
                ViewBag.Currentrole = currentrole;
                editBugReply.ListofAttachments = _bugQueries.GetListReplyAttachmentsByAttachmentId(editBugReply.BugId, editBugReply.BugReplyId);


                return View(editBugReply);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void EditReplyEmailtoSend(long? bugId, string description, string repliedby)
        {
            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "bugreply.html");
                var recipientdetails = _bugQueries.GetBugTesterbybugId(bugId);
                string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/BugDetails/Details/{bugId}";

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
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Description</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{description}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Updated by</th>");
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
                    Subject = $"[Bug {bugId}] [Edited Reply]"
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

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
    }
}
