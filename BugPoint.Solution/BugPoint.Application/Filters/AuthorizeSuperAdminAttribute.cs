using System;
using BugPoint.Application.Notification;
using BugPoint.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BugPoint.Application.Filters
{
    public class AuthorizeSuperAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.SuperAdmin))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeSuperAdminAndAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.SuperAdmin) && roleValue != Convert.ToInt32(RolesHelper.Roles.Admin))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeDeveloperandLeadAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.Developer) && roleValue != Convert.ToInt32(RolesHelper.Roles.DeveloperTeamLead))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeTesterandLeadAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.Tester) && roleValue != Convert.ToInt32(RolesHelper.Roles.TesterTeamLead)
                                                                           && roleValue != Convert.ToInt32(RolesHelper.Roles.Reporter))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeProjectManagerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.ProjectManager))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.Admin))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeDeveloperAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.Developer))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeDeveloperLeadAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.DeveloperTeamLead))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeTesterLeadAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.TesterTeamLead))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeTesterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.Tester) && roleValue != Convert.ToInt32(RolesHelper.Roles.Reporter))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthenticateDeveloperAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                var bugid = Convert.ToString(context.RouteData.Values["id"]);

                if (string.IsNullOrEmpty(bugid))
                {
                    if (roleValue == Convert.ToInt32(RolesHelper.Roles.Developer))
                    {
                        context.Result = new RedirectResult("/MyBugList/Show");
                    }
                    else if (roleValue == Convert.ToInt32(RolesHelper.Roles.DeveloperTeamLead))
                    {
                        context.Result = new RedirectResult("/MyBugList/AllReportedBugs");
                    }
                }
            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthenticateTesterAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                var bugid = Convert.ToString(context.RouteData.Values["id"]);

                if (string.IsNullOrEmpty(bugid))
                {
                    if (roleValue != Convert.ToInt32(RolesHelper.Roles.Tester))
                    {
                        context.Result = new RedirectResult("/BugList/Show");
                    }
                    else if (roleValue != Convert.ToInt32(RolesHelper.Roles.TesterTeamLead))
                    {
                        context.Result = new RedirectResult("/BugList/AllReportedBugs");
                    }
                }
            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }
}