using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Common;
using BugPoint.Data.Audit.Queries;
using BugPoint.Data.UserMaster.Command;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.ViewModel.Login;
using BugPoint.ViewModel.UserMaster;
using Microsoft.AspNetCore.Http;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    public class MyAccountController : Controller
    {
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IUserMasterCommand _userMasterCommand;
        private readonly INotificationService _notificationService;
        private IAuditQueries _auditQueries;
        public MyAccountController(IUserMasterQueries userMasterQueries,
            IUserMasterCommand userMasterCommand,
            INotificationService notificationService, IAuditQueries auditQueries)
        {
            _userMasterQueries = userMasterQueries;
            _userMasterCommand = userMasterCommand;
            _notificationService = notificationService;
            _auditQueries = auditQueries;
        }

        [HttpGet]
        public ActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var userprofile = _userMasterQueries.UserProfile(userId);
            var profileViewModel = new ProfileViewModel()
            {
                Profile = userprofile,
                ListofActivites = _auditQueries.GetUserActivity(userId)
            };

            return View(profileViewModel);
        }

        [HttpGet]
        public ActionResult Changepassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Changepassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                    var storedUserpassword = _userMasterQueries.GetUserById(user);

                    if (changePasswordViewModel.OldPassword == changePasswordViewModel.NewPassword)
                    {
                        _notificationService.DangerNotification("Message", "New Password Cannot be same as Old Password!");
                        return View(changePasswordViewModel);
                    }
                    else if (!string.Equals(storedUserpassword.PasswordHash, changePasswordViewModel.OldPassword, StringComparison.Ordinal))
                    {
                        _notificationService.DangerNotification("Message", "Current Password Entered is InValid!");
                        return View(changePasswordViewModel);
                    }
                    else if (!string.Equals(changePasswordViewModel.NewPassword, changePasswordViewModel.ConfirmPassword, StringComparison.Ordinal))
                    {
                        _notificationService.DangerNotification("Message", "Password Does not Match!");
                        return View(changePasswordViewModel);
                    }
                    else if (changePasswordViewModel.OldPassword.Length != 64 || changePasswordViewModel.NewPassword.Length != 64 || changePasswordViewModel.ConfirmPassword.Length != 64)
                    {
                        _notificationService.DangerNotification("Message", "Invalid Password Refresh Page and Try once Again!");
                        return View(changePasswordViewModel);
                    }
                    else if (PasswordDictionary.CheckCommonPassword(changePasswordViewModel.NewPassword))
                    {
                        _notificationService.DangerNotification("Message", "Entered Password is Common. Try another Password");
                        return View(changePasswordViewModel);
                    }
                    else
                    {
                        // First Login
                        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Portal.IsFirstLogin")))
                        {
                            HttpContext.Session.SetString("Portal.IsFirstLogin", "0");

                            var result = _userMasterCommand.UpdatePasswordandHistory(user, changePasswordViewModel.ConfirmPassword, "F");
                            if (result)
                            {
                                _notificationService.SuccessNotification("Message", "Your Password Changed Successfully!");
                            }
                            else
                            {
                                _notificationService.DangerNotification("Message", "Something Went Wrong While changing your Password Please try again!");
                            }
                        }
                        // Change Password
                        else
                        {
                            var result = _userMasterCommand.UpdatePasswordandHistory(user, changePasswordViewModel.ConfirmPassword, "C");
                            if (result)
                            {
                                _notificationService.SuccessNotification("Message", "Your Password Changed Successfully!");
                            }
                            else
                            {
                                _notificationService.DangerNotification("Message", "Something Went Wrong While changing your Password Please try again!");
                            }
                        }

                        return RedirectToAction("Changepassword", "MyAccount");
                    }
                }
                return View(changePasswordViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
