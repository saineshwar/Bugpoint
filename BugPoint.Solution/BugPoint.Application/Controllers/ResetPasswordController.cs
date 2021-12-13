using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Common;
using BugPoint.Data.UserMaster.Command;
using BugPoint.Data.UserMaster.Queries;
using BugPoint.ViewModel.Login;
using BugPoint.ViewModel.UserMaster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BugPoint.Application.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IVerificationCommand _verificationCommand;
        private readonly IUserMasterCommand _userMasterCommand;
        private readonly ILogger<VerifyResetPasswordController> _logger;
        public ResetPasswordController(IUserMasterQueries userMasterQueries, IVerificationCommand verificationCommand, IUserMasterCommand userMasterCommand, ILogger<VerifyResetPasswordController> logger)
        {
            _userMasterQueries = userMasterQueries;
            _verificationCommand = verificationCommand;
            _userMasterCommand = userMasterCommand;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Reset()
        {
            var userid = HttpContext.Session.GetInt32("VerificationUserId");
            var verificationGeneratedToken = HttpContext.Session.GetString("VerificationGeneratedToken");

            if (userid == null || string.IsNullOrEmpty(verificationGeneratedToken))
            {
                return RedirectToAction("Login", "Portal");
            }

            return View(new ResetPasswordViewModel());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reset(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userid = HttpContext.Session.GetInt32("VerificationUserId");
                    var verificationGeneratedToken = HttpContext.Session.GetString("VerificationGeneratedToken");

                    if (userid == null || string.IsNullOrEmpty(verificationGeneratedToken))
                    {
                        return RedirectToAction("Login", "Portal");
                    }

                    var getuserdetails = _userMasterQueries.GetUserDetailsbyUserId(userid);

                    if (!string.Equals(resetPasswordViewModel.Password, resetPasswordViewModel.ConfirmPassword, StringComparison.Ordinal))
                    {
                        TempData["Reset_Error_Message"] = "Password Does not Match";
                        return View(resetPasswordViewModel);
                    }
                    else if (PasswordDictionary.CheckCommonPassword(resetPasswordViewModel.Password))
                    {
                        TempData["Reset_Error_Message"] = "Entered Password is Common. Try another Password";
                        return View(resetPasswordViewModel);
                    }
                    else
                    {
                        var updateResetPasswordVerification =
                            new UpdateResetPasswordVerification()
                            {
                                UserId = userid,
                                GeneratedToken = verificationGeneratedToken,
                                Password = resetPasswordViewModel.Password
                            };

                        var result = _verificationCommand.UpdatePasswordandVerificationStatus(updateResetPasswordVerification);

                        if (getuserdetails.IsFirstLogin)
                        {
                            _userMasterCommand.UpdateIsFirstLoginStatus(userid);
                        }

                        if (result == "Success")
                        {
                            TempData["Reset_Success_Message"] = "Password Reset Successfully!";
                            return RedirectToAction("Login", "Portal");
                        }
                        else
                        {
                            TempData["Reset_Error_Message"] = "Something Went Wrong Please try again!";
                            return View(resetPasswordViewModel);
                        }

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ResetPasswordController:Reset");
                    throw;
                }
            }

            return View(resetPasswordViewModel);
        }

    }
}
