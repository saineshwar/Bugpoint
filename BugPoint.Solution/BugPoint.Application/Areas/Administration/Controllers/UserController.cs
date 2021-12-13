using AutoMapper;
using BugPoint.Common;
using BugPoint.Data.RoleMaster.Queries;
using BugPoint.Data.UserMaster.Command;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.Model.UserMaster;
using BugPoint.ViewModel.UserMaster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BugPoint.Application.Filters;
using BugPoint.Application.Helpers;
using BugPoint.Application.Notification;
using BugPoint.Data.Masters.Queries;
using BugPoint.Services.MailHelper;
using BugPoint.ViewModel.Login;
using Microsoft.AspNetCore.Hosting;

namespace BugPoint.Application.Areas.Administration.Controllers
{
    [Area("Administration")]
    [SessionTimeOut]
    [AuthorizeSuperAdminAndAdminAttribute]
    public class UserController : Controller
    {
        private readonly IRoleQueries _roleQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IUserMasterCommand _userMasterCommand;
        private readonly IMapper _mapper;
        private readonly IAssignedRolesQueries _assignedRolesQueries;
        private readonly INotificationService _notificationService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailingService _mailingService;
        private readonly IVerificationCommand _verificationCommand;
        private readonly IMastersQueries _mastersQueries;

        public UserController(IRoleQueries roleQueries,
            IUserMasterQueries userMasterQueries,
            IUserMasterCommand userMasterCommand,
            IMapper mapper,
            IAssignedRolesQueries assignedRolesQueries,
            INotificationService notificationService, IWebHostEnvironment webHostEnvironment, IMailingService mailingService, IVerificationCommand verificationCommand, IMastersQueries mastersQueries)
        {
            _roleQueries = roleQueries;
            _userMasterQueries = userMasterQueries;
            _userMasterCommand = userMasterCommand;
            _mapper = mapper;
            _assignedRolesQueries = assignedRolesQueries;
            _notificationService = notificationService;
            _webHostEnvironment = webHostEnvironment;
            _mailingService = mailingService;
            _verificationCommand = verificationCommand;
            _mastersQueries = mastersQueries;
        }

        public IActionResult Create()
        {
            try
            {
                var createUserViewModel = new CreateUserViewModel()
                {
                    ListofRoles = _roleQueries.GetAllActiveRoles(),
                    ListofDesignation = _mastersQueries.ListofDesignation()
                };
                return View(createUserViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel createUserViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_userMasterQueries.CheckEmailIdExists(createUserViewModel.EmailId))
                    {
                        ModelState.AddModelError("", "EmailId Already Exists");
                    }
                    else if (_userMasterQueries.CheckMobileNoExists(createUserViewModel.MobileNo))
                    {
                        ModelState.AddModelError("", "MobileNo Already Exists");
                    }
                    else if (_userMasterQueries.CheckUsernameExists(createUserViewModel.UserName))
                    {
                        ModelState.AddModelError("", "Username already exists");
                    }
                    else
                    {
                        createUserViewModel.ListofRoles = _roleQueries.GetAllActiveRoles();
                        createUserViewModel.ListofDesignation = _mastersQueries.ListofDesignation();

                        createUserViewModel.FirstName =
                            UppercaseFirstHelper.UppercaseFirst(createUserViewModel.FirstName);
                        createUserViewModel.LastName =
                            UppercaseFirstHelper.UppercaseFirst(createUserViewModel.LastName);

                        var usermaster = _mapper.Map<UserMasterModel>(createUserViewModel);
                        usermaster.Status = true;
                        usermaster.DesignationId = createUserViewModel.DesignationId;
                        usermaster.CreatedOn = DateTime.Now;
                        usermaster.UserId = 0;
                        usermaster.IsFirstLoginDate = DateTime.Now;
                        usermaster.CreatedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId);

                        if (!string.Equals(createUserViewModel.Password, createUserViewModel.ConfirmPassword,
                            StringComparison.Ordinal))
                        {
                            _notificationService.DangerNotification("Message", "Password Does not Match!");
                            return View(createUserViewModel);
                        }
                        else
                        {
                            usermaster.PasswordHash = createUserViewModel.Password;

                            var userId = _userMasterCommand.AddUser(usermaster,
                                createUserViewModel.RoleId);
                            if (userId != -1)
                            {

                                _notificationService.SuccessNotification("Message", "User Created Successfully");
                                var token = HashHelper.CreateHashSHA256((GenerateRandomNumbers.GenerateRandomDigitCode(6)));
                                var commonDatetime = DateTime.Now;

                                if (userId != null)
                                {
                                    ResetPasswordVerification resetPasswordVerification = new ResetPasswordVerification()
                                    {
                                        UserId = userId.Value,
                                        GeneratedDate = commonDatetime,
                                        GeneratedToken = token
                                    };
                                    var emailresult = _verificationCommand.InsertResetPasswordVerificationToken(resetPasswordVerification);
                                }

                                var sendingresult = CreateVerificationEmail(usermaster, token, commonDatetime);

                            }

                            return RedirectToAction("Index", "User");
                        }
                    }

                    createUserViewModel.ListofRoles = _roleQueries.GetAllActiveRoles();
                    createUserViewModel.ListofDesignation = _mastersQueries.ListofDesignation();
                    return View("Create", createUserViewModel);
                }
                else
                {
                    createUserViewModel.ListofRoles = _roleQueries.GetAllActiveRoles();
                    createUserViewModel.ListofDesignation = _mastersQueries.ListofDesignation();
                    return View("Create", createUserViewModel);
                }
            }
            catch
            {
                throw;
            }
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllUser()
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


                var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);

                if (currentrole == Convert.ToInt32(RolesHelper.Roles.Admin))
                {
                    var usersdata = _userMasterQueries.ShowAllUsersAdmin(sortColumn, sortColumnDirection, searchValue);
                    recordsTotal = usersdata.Count();
                    var data = usersdata.Skip(skip).Take(pageSize).ToList();
                    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                    return Ok(jsonData);
                }

                if (currentrole == Convert.ToInt32(RolesHelper.Roles.SuperAdmin))
                {
                    var usersdata = _userMasterQueries.ShowAllUsers(sortColumn, sortColumnDirection, searchValue);
                    recordsTotal = usersdata.Count();
                    var data = usersdata.Skip(skip).Take(pageSize).ToList();
                    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                    return Ok(jsonData);
                }

                return Ok(null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "User");
                }
                var userMaster = _userMasterQueries.GetUserForEditByUserId(id);
                userMaster.ListRole = _roleQueries.ListofRoles();
                userMaster.ListofDesignation = _mastersQueries.ListofDesignation();
                return View(userMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditUserViewModel editUser)
        {
            editUser.ListRole = _roleQueries.ListofRoles();
            editUser.ListofDesignation = _mastersQueries.ListofDesignation();

            if (!ModelState.IsValid)
            {
                return View(editUser);
            }

            var userMaster = _userMasterQueries.GetUserDetailsbyUserId(editUser.UserId);

            if (editUser.MobileNo != userMaster.MobileNo)
            {
                if (_userMasterQueries.CheckMobileNoExists(editUser.MobileNo))
                {
                    ModelState.AddModelError("", "Entered MobileNo Already Exists");
                    return View(editUser);
                }
            }

            if (editUser.EmailId != userMaster.EmailId)
            {
                if (_userMasterQueries.CheckEmailIdExists(editUser.EmailId))
                {
                    ModelState.AddModelError("", "Entered EmailId Already Exists");
                    return View(editUser);
                }
            }

            userMaster.DesignationId = editUser.DesignationId;
            userMaster.FirstName = editUser.FirstName;
            userMaster.LastName = editUser.LastName;
            userMaster.MobileNo = editUser.MobileNo;
            userMaster.EmailId = editUser.EmailId;
            userMaster.Gender = editUser.Gender;
            userMaster.ModifiedOn = DateTime.Now;
            userMaster.ModifiedBy = Convert.ToInt32(HttpContext.Session.GetInt32(AllSessionKeys.UserId));

            var assignedroles = _assignedRolesQueries.GetAssignedRolesDetailsbyUserId(userMaster.UserId);
            assignedroles.RoleId = editUser.RoleId;

            var result = _userMasterCommand.UpdateUser(userMaster, assignedroles);

            if (result == "success")
            {
                _notificationService.SuccessNotification("Message", "User Details Updated Successfully !");
                return RedirectToAction("Index");
            }
            else
            {
                _notificationService.DangerNotification("Message", "Something went wrong Please Try Once Again !");
                return View(editUser);
            }
        }

        public JsonResult ChangeUserStatus(RequestStatus requestStatus)
        {
            try
            {
                var userMaster = _userMasterQueries.GetUserDetailsbyUserId(requestStatus.UserId);
                userMaster.Status = requestStatus.Status;
                var result = _userMasterCommand.ChangeUserStatus(userMaster);

                if (result == "success")
                {
                    _notificationService.SuccessNotification("Message", "Changed User Status successfully!");
                    return Json(new { Result = "success" });
                }
                else
                {
                    return Json(new { Result = "failed", Message = "Cannot Delete" });
                }

            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }

        private string CreateVerificationEmail(UserMasterModel user, string token, DateTime commonDateTime)
        {
            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "newUser.html");

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var aesAlgorithm = new AesAlgorithm();
                var key = string.Join(":", new string[] { commonDateTime.Ticks.ToString(), user.UserId.ToString() });
                var encrypt = aesAlgorithm.EncryptToBase64String(key);

                string linktoverify = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/VerifyResetPassword/Verify?key={HttpUtility.UrlEncode(encrypt)}&hashtoken={HttpUtility.UrlEncode(token)}";

                var stringtemplate = new StringBuilder();
                stringtemplate.Append("Your account has been successfully created!");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append($"Your Username: {user.UserName}");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("Please click the following link to set your password.");
                stringtemplate.Append("<br/>");
                stringtemplate.Append($"Set password link : <a target='_blank' href={linktoverify}>Click here to set your password</a>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("If the link does not work, copy and paste the URL into a new browser window. The URL will expire in 24 hours for security reasons.");
                stringtemplate.Append("<br/>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", stringtemplate.ToString());
                mailText = mailText.Replace("[##Name##]", $"{ user.FirstName} { user.LastName}");

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = user.EmailId,
                    Body = mailText,
                    Subject = $"Welcome to BugPoint"
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

                return "success";
            }
            catch (Exception)
            {
                return "failed";
                throw;
            }
        }
    }
}
