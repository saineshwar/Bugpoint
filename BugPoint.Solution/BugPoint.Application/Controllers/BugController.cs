using AutoMapper;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.Assigned.Queries;
using BugPoint.Data.Bugs.Command;
using BugPoint.Data.Bugs.Queries;
using BugPoint.Data.Masters.Queries;
using BugPoint.Data.Project.Queries;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.Model.Bugs;
using BugPoint.Services.AwsHelper;
using BugPoint.Services.MailHelper;
using BugPoint.ViewModel.Bugs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    [AuthorizeTesterandLead]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class BugController : Controller
    {
        private readonly IMastersQueries _mastersQueries;
        private readonly IProjectQueries _projectQueries;
        private readonly IProjectComponentQueries _projectComponentQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IBugNumberGeneratorQueries _bugNumberGeneratorQueries;
        private readonly IMapper _mapper;
        private readonly IBugCommand _bugCommand;
        private readonly INotificationService _notificationService;
        private readonly IBugQueries _bugQueries;
        private readonly IBugHistoryCommand _bugHistoryCommand;
        private readonly IBugHistoryHelper _bugHistoryHelper;
        private readonly IAssignProjectQueries _assignProjectQueries;
        private readonly IMailingService _mailingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAwsS3HelperService _awsS3HelperService;
        private readonly AwsSettings _awsSettings;
        private readonly AppSettingsProperties _appsettings;

        public BugController(IMastersQueries mastersQueries,
            IProjectQueries projectQueries,
            IProjectComponentQueries projectComponentQueries,
            IUserMasterQueries userMasterQueries,
            IBugNumberGeneratorQueries bugNumberGeneratorQueries,
            IMapper mapper,
            IBugCommand bugCommand,
            INotificationService notificationService,
            IBugQueries bugQueries, IBugHistoryCommand bugHistoryCommand,
            IBugHistoryHelper bugHistoryHelper, IAssignProjectQueries assignProjectQueries,
            IMailingService mailingService, IWebHostEnvironment webHostEnvironment,
            IAwsS3HelperService awsS3HelperService,
            IOptions<AwsSettings> awsSettings,
            IOptions<AppSettingsProperties> appsettings
            )
        {
            _mastersQueries = mastersQueries;
            _projectQueries = projectQueries;
            _projectComponentQueries = projectComponentQueries;
            _userMasterQueries = userMasterQueries;
            _bugNumberGeneratorQueries = bugNumberGeneratorQueries;
            _mapper = mapper;
            _bugCommand = bugCommand;
            _notificationService = notificationService;
            _bugQueries = bugQueries;
            _bugHistoryCommand = bugHistoryCommand;
            _bugHistoryHelper = bugHistoryHelper;
            _assignProjectQueries = assignProjectQueries;
            _mailingService = mailingService;
            _webHostEnvironment = webHostEnvironment;
            _awsS3HelperService = awsS3HelperService;
            _awsSettings = awsSettings.Value;
            _appsettings = appsettings.Value;
        }

        public IActionResult Add()
        {

            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var bug = new BugViewModel();
            bug.ListofProjects = _projectQueries.GetAssignedProjectList(user);
            bug.ListofBrowser = _mastersQueries.ListofBrowser();
            bug.ListofComponents = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "", Text = "-----Select-----"
                }

            };
            bug.ListofHardware = _mastersQueries.ListofHardware();
            bug.ListofWebFramework = _mastersQueries.ListofWebFramework();
            bug.ListofHardware = _mastersQueries.ListofHardware();
            bug.ListofOperatingSystem = _mastersQueries.ListofOperatingSystem();
            bug.ListofVersion = _mastersQueries.ListofVersions();
            bug.ListofBugType = _mastersQueries.ListofBugTypes();
            bug.ListofSeverity = _mastersQueries.ListofSeverity();
            bug.ListofPriority = _mastersQueries.ListofPriority();
            bug.ListofTestedOn = _mastersQueries.ListofEnvironments();

            bug.ListofUsers = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "", Text = "-----Select-----"
                }
            };

            return View(bug);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BugViewModel bug)
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

            if (ModelState.IsValid)
            {
                #region Inserting Data

                var createdOn = DateTime.Now;

                var bugId = _bugNumberGeneratorQueries.GenerateNo();
                var bugSummaryModel = _mapper.Map<BugSummaryModel>(bug);
                bugSummaryModel.BugSummaryId = 0;
                bugSummaryModel.BugId = bugId;
                bugSummaryModel.CreatedOn = createdOn;
                bugSummaryModel.CreatedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

                var bugDetailsModel = new BugDetailsModel()
                {
                    BugId = bugId,
                    BugDetailsId = 0,
                    Description = WebUtility.HtmlDecode(bug.Description)
                };

                var bugTrackingModel = new BugTrackingModel()
                {
                    BugId = bugId,
                    StatusId = Convert.ToInt32(StatusHelper.Status.NEW),
                    CreatedOn = createdOn,
                    BugTrackingId = 0,
                    CreatedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId),
                    AssignedTo = bug.AssignBugto
                };

                var userdetails = _userMasterQueries.GetUserById(bug.AssignBugto);
                var username = $"{userdetails.FirstName} {userdetails.LastName}";

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
                            attachments.BugId = bugId;
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

                var result = _bugCommand.AddBug(bugSummaryModel, bugDetailsModel, bugTrackingModel, listofattachments);

                if (result)
                {

                    string priorityName = _mastersQueries.GetPriorityNameBypriorityId(bug.PriorityId);
                    string statusname = _mastersQueries.GetStatusBystatusId(Convert.ToInt32(StatusHelper.Status.NEW));

                    var bugHistory = new BugHistoryModel
                    {
                        UserId = user,
                        Message = _bugHistoryHelper.CreateBug(username, statusname, priorityName),
                        ProcessDate = DateTime.Now,
                        BugId = bugId,
                        PriorityId = null,
                        StatusId = null,
                        AssignedTo = bug.AssignBugto
                    };

                    _bugHistoryCommand.InsertBugHistory(bugHistory);

                    _notificationService.SuccessNotification("Message",
                        $"Bug Created Successfully. Your BugId is:- {bugId}");

                    var emailid = _userMasterQueries.GetUserById(bug.AssignBugto);


                    var sendingMailRequest = new SendingMailRequest()
                    {
                        Attachments = null,
                        ToEmail = emailid.EmailId,
                        Body = CreateEmailtoSend(bugId, username),
                        Subject = $"[Bug {bugId}] [New] : {bug.Summary}"
                    };

                    var mailresult = _mailingService.SendEmailAsync(sendingMailRequest);

                    return RedirectToAction("Show", "BugList");
                }

                #endregion

            }

            bug.ListofProjects = _projectQueries.GetAssignedProjectList(user);
            bug.ListofBrowser = _mastersQueries.ListofBrowser();
            bug.ListofComponents = _projectComponentQueries.GetProjectComponentsList(bug.ProjectId);
            bug.ListofHardware = _mastersQueries.ListofHardware();
            bug.ListofWebFramework = _mastersQueries.ListofWebFramework();
            bug.ListofHardware = _mastersQueries.ListofHardware();
            bug.ListofOperatingSystem = _mastersQueries.ListofOperatingSystem();
            bug.ListofVersion = _mastersQueries.ListofVersions();
            bug.ListofBugType = _mastersQueries.ListofBugTypes();
            bug.ListofSeverity = _mastersQueries.ListofSeverity();
            bug.ListofPriority = _mastersQueries.ListofPriority();
            bug.ListofTestedOn = _mastersQueries.ListofEnvironments();
            bug.ListofUsers = _userMasterQueries.GetListofDevelopers(Convert.ToInt32(bug.ProjectId));

            return View(bug);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var bugsummary = _bugQueries.GetBugSummarybybugId(id);
            var bugdetails = _bugQueries.GetBugsDetailsbybugId(id);
            var bugtrackingdetails = _bugQueries.GetBugTrackingbybugId(id);
            var editBugViewModel = _mapper.Map<EditBugViewModel>(bugsummary);
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var componentDetails =
                _projectComponentQueries.ProjectComponentDetailsByProjectId(
                    Convert.ToInt32(bugsummary.ProjectComponentId));

            editBugViewModel.BugSummaryId = bugsummary.BugSummaryId;
            editBugViewModel.BugDetailsId = bugdetails.BugDetailsId;
            editBugViewModel.BugTrackingId = bugtrackingdetails.BugTrackingId;

            editBugViewModel.Description = bugdetails.Description;
            editBugViewModel.ComponentDescription = componentDetails.ComponentDescription;
            editBugViewModel.ListofProjects = _projectQueries.GetAssignedProjectList(user);
            editBugViewModel.ListofBrowser = _mastersQueries.ListofBrowser();
            editBugViewModel.ListofComponents =
                _projectComponentQueries.GetProjectComponentsList(editBugViewModel.ProjectId);
            editBugViewModel.ListofHardware = _mastersQueries.ListofHardware();
            editBugViewModel.ListofWebFramework = _mastersQueries.ListofWebFramework();
            editBugViewModel.ListofHardware = _mastersQueries.ListofHardware();
            editBugViewModel.ListofOperatingSystem = _mastersQueries.ListofOperatingSystem();
            editBugViewModel.ListofVersion = _mastersQueries.ListofVersions();
            editBugViewModel.ListofBugType = _mastersQueries.ListofBugTypes();
            editBugViewModel.ListofSeverity = _mastersQueries.ListofSeverity();
            editBugViewModel.ListofPriority = _mastersQueries.ListofPriority();
            editBugViewModel.ListofTestedOn = _mastersQueries.ListofEnvironments();
            editBugViewModel.ListofUsers =
                _userMasterQueries.GetListofDevelopers(Convert.ToInt32(editBugViewModel.ProjectId));
            editBugViewModel.AssignBugto = bugtrackingdetails.AssignedTo;
            editBugViewModel.ListofStatus = _mastersQueries.ListofStatus();
            editBugViewModel.StatusId = bugtrackingdetails.StatusId;
            editBugViewModel.ListofAttachments = _bugQueries.GetListAttachmentsBybugId(id);

            return View(editBugViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBugViewModel bug)
        {

            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var currentdate = DateTime.Now;
            bug.ListofProjects = _projectQueries.GetAssignedProjectList(user);
            bug.ListofBrowser = _mastersQueries.ListofBrowser();
            bug.ListofComponents = _projectComponentQueries.GetProjectComponentsList(bug.ProjectId);
            bug.ListofHardware = _mastersQueries.ListofHardware();
            bug.ListofWebFramework = _mastersQueries.ListofWebFramework();
            bug.ListofHardware = _mastersQueries.ListofHardware();
            bug.ListofOperatingSystem = _mastersQueries.ListofOperatingSystem();
            bug.ListofVersion = _mastersQueries.ListofVersions();
            bug.ListofBugType = _mastersQueries.ListofBugTypes();
            bug.ListofSeverity = _mastersQueries.ListofSeverity();
            bug.ListofPriority = _mastersQueries.ListofPriority();
            bug.ListofTestedOn = _mastersQueries.ListofEnvironments();
            bug.ListofUsers = _userMasterQueries.GetListofDevelopers(Convert.ToInt32(bug.ProjectId));

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Any() && (_bugQueries.AttachmentsExistbybugId(bug.BugId)))
                {
                    _notificationService.DangerNotification("Message",
                        "All Delete Pervious uploaded Attachments for Uploading New Attachments");
                    return View(bug);
                }

                var bugSummaryModel = _bugQueries.GetBugSummarybybugId(bug.BugId);
                bugSummaryModel.Summary = bug.Summary;
                bugSummaryModel.ProjectId = bug.ProjectId;
                bugSummaryModel.ProjectComponentId = bug.ProjectComponentId;
                bugSummaryModel.SeverityId = bug.SeverityId;
                bugSummaryModel.PriorityId = bug.PriorityId;
                bugSummaryModel.VersionId = bug.VersionId;
                bugSummaryModel.OperatingSystemId = bug.OperatingSystemId;
                bugSummaryModel.HardwareId = bug.HardwareId;
                bugSummaryModel.BrowserId = bug.BrowserId;
                bugSummaryModel.WebFrameworkId = bug.WebFrameworkId;
                bugSummaryModel.TestedOnId = bug.TestedOnId;
                bugSummaryModel.BugTypeId = bug.BugTypeId;
                bugSummaryModel.Urls = bug.Urls;
                bugSummaryModel.ModifiedOn = currentdate;
                bugSummaryModel.ModifiedBy = user;

                var bugdetails = _bugQueries.GetBugsDetailsbybugId(bug.BugId);
                bugdetails.Description = WebUtility.HtmlDecode(bug.Description);

                var bugtrackingdetails = _bugQueries.GetBugTrackingbybugId(bug.BugId);

                bugtrackingdetails.ModifiedOn = currentdate;
                bugtrackingdetails.ModifiedBy = user;
                bugtrackingdetails.AssignedTo = bug.AssignBugto;

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
                            attachments.BugId = bug.BugId;
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


                var result = _bugCommand.UpdateBug(bugSummaryModel, bugdetails, bugtrackingdetails, listofattachments);

                if (result)
                {

                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);

                    var bugHistory = new BugHistoryModel
                    {
                        UserId = user,
                        Message = _bugHistoryHelper.EditBugMessage(fullName, bugSummaryModel.BugId),
                        ProcessDate = DateTime.Now,
                        BugId = bugSummaryModel.BugId,
                        PriorityId = null,
                        StatusId = null,
                        AssignedTo = null
                    };

                    _bugHistoryCommand.InsertBugHistory(bugHistory);

                    _notificationService.SuccessNotification("Message", $"Bug Updated Successfully.");

                    var emailid = _userMasterQueries.GetUserById(bug.AssignBugto);

                    var sendingMailRequest = new SendingMailRequest()
                    {
                        Attachments = null,
                        ToEmail = emailid.EmailId,
                        Body = EditEmailtoSend(bugSummaryModel.BugId),
                        Subject = $"[Bug { bugSummaryModel.BugId}] [Edited]"
                    };

                    _mailingService.SendEmailAsync(sendingMailRequest);

                    return RedirectToAction("Show", "BugList");
                }

            }

            return View(bug);
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
                            "All Delete Pervious uploaded Attachments for Uploading New Attachments");
                        return RedirectToAction("EditReply", "Bug", new { id = editBugReply.BugId, rid = editBugReply.BugReplyId });
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
                        return RedirectToAction("Details", "BugDetails", new { id = editBugReply.BugId });
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

        public IActionResult GetProjectComponents(RequestProjectComponent requestProject)
        {
            var listofprojects = _projectComponentQueries.GetProjectComponentsList(requestProject.ProjectId);
            var project = _projectComponentQueries.ProjectComponentDetailsByProjectId(requestProject.ProjectId);
            return Json(new { projectcollection = listofprojects });
        }

        public IActionResult GetProjectDescription(RequestProjectComponentDesc requestProject)
        {
            var projectdescription =
                _projectComponentQueries.ProjectComponentDetailsByProjectId(requestProject.ProjectComponentId);
            return Json(new { ComponentDescription = projectdescription.ComponentDescription });
        }

        public IActionResult GetUserList(RequestProjectComponent requestProject)
        {

            var roleValue = Convert.ToInt32(HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
            if (roleValue == Convert.ToInt32(RolesHelper.Roles.Tester))
            {
                var listofusers = _assignProjectQueries.GetDevelopersandTeamLeadAssignedtoProject(requestProject.ProjectId);
                return Json(new { listofusers = listofusers });
            }
            else if (roleValue == Convert.ToInt32(RolesHelper.Roles.Reporter))
            {
                var listofusers = _assignProjectQueries.GetDeveloperTeamLeadAssignedtoProject(requestProject.ProjectId);
                return Json(new { listofusers = listofusers });
            }

            return Json(new { listofusers = "" });
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

                    return RedirectToAction("Show", "BugList");
                }
                else
                {
                    return RedirectToAction("Show", "BugList");
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

        public IActionResult GetBugActivities(RequestBugReopen requestBug)
        {
            var listofbugHistory = _bugQueries.GetBugHistorybyBugId(requestBug.BugId);
            return PartialView("_BugActivities", listofbugHistory);
        }

        public IActionResult GetAssignedtoProjectReporter(RequestProjectReporter requestProject)
        {

            var listofusers =
                _assignProjectQueries.GetTestersandDevelopersAssignedtoProject(requestProject.ProjectId,
                    (int)RolesHelper.Roles.Tester);


            return Json(listofusers);
        }

        private string CreateEmailtoSend(int bugId, string assignto)
        {
            try
            {
                var bugDetailViewModel = _bugQueries.GetBugDetailsbyBugId(bugId);

                string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/MyBugDetails/Details/{bugId}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "newbug.html");

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
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{url}'>{bugDetailViewModel.BugId}</a> </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Bug Summary</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Summary}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Bug Description</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Description}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Project Component</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.ProjectComponent}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Version</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Version}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Hardware</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Hardware}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>URL</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{bugDetailViewModel.Urls}'>{bugDetailViewModel.Urls}</a></td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Operating System</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.OperatingSystemName}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Status</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.StatusName}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Priority</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Priority}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Severity</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Severity}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Assignee</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.CreatedBy}</td>");
                str.Append("</tr>");
                str.Append("</tbody></table>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", assignto);
                mailText = mailText.Replace("[##Message##]", "New Bug is Assigned to You.");

                return mailText;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string EditEmailtoSend(long? bugId)
        {
            try
            {
                var bugDetailViewModel = _bugQueries.GetBugDetailsbyBugId(bugId);

                string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/MyBugDetails/Details/{bugId}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "newbug.html");

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
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{url}'>{bugDetailViewModel.BugId}</a> </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Bug Summary</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Summary}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Bug Description</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Description}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Project Component</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.ProjectComponent}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Version</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Version}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Hardware</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Hardware}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>URL</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{bugDetailViewModel.Urls}'>{bugDetailViewModel.Urls}</a></td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Operating System</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.OperatingSystemName}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Status</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.StatusName}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Priority</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Priority}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Severity</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.Severity}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Assignee</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{bugDetailViewModel.CreatedBy}</td>");
                str.Append("</tr>");
                str.Append("</tbody></table>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", bugDetailViewModel.AssignedTo);
                mailText = mailText.Replace("[##Message##]", "Edited Bug which is Assigned to You.");

                return mailText;
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
                var recipientdetails = _bugQueries.GetBugAssigntobybugId(bugId);
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
    }
}
