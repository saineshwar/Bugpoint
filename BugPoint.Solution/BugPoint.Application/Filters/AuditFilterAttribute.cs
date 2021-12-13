using System;
using BugPoint.Common;
using BugPoint.Data.Audit.Command;
using BugPoint.Model.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BugPoint.Application.Filters
{
    public class AuditFilterAttribute : ActionFilterAttribute
    {
        private readonly IAuditCommand _auditCommand;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuditFilterAttribute(IAuditCommand auditCommand, IHttpContextAccessor httpContextAccessor)
        {
            _auditCommand = auditCommand;
            _httpContextAccessor = httpContextAccessor;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {

                var objaudit = new AuditModel(); // Getting Action Name 

                var controllerName = ((ControllerBase)filterContext.Controller)
                    .ControllerContext.ActionDescriptor.ControllerName;
                var actionName = ((ControllerBase)filterContext.Controller)
                    .ControllerContext.ActionDescriptor.ActionName;
                var area = ((ControllerBase)filterContext.Controller)
                    .ControllerContext.ActionDescriptor.RouteValues["area"];

                var request = filterContext.HttpContext.Request;

                if (area != null)
                {
                    objaudit.Area = Convert.ToString(area);
                }

                objaudit.LangId = null;

                if (!string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session.GetInt32(AllSessionKeys.UserId))))
                {
                    objaudit.UserID = filterContext.HttpContext.Session.GetInt32(AllSessionKeys.UserId);
                }
                else
                {
                    objaudit.UserID = null;
                }

                if (!string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
                {
                    objaudit.RoleId = filterContext.HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
                }
                else
                {
                    objaudit.RoleId = null;
                }

                objaudit.PortalToken = null;
                objaudit.IsFirstLogin = false;

                objaudit.SessionID = filterContext.HttpContext.Session.Id; ; // Application SessionID // User IPAddress 
                if (_httpContextAccessor.HttpContext != null)
                    objaudit.IPAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

                objaudit.PageAccessed = Convert.ToString(filterContext.HttpContext.Request.Path); // URL User Requested 

                objaudit.LoginStatus = "A";
                objaudit.ControllerName = controllerName; // ControllerName 
                objaudit.ActionName = actionName;
                objaudit.CurrentDatetime = DateTime.Now;
                RequestHeaders header = request.GetTypedHeaders();
                Uri uriReferer = header.Referer;

                if (uriReferer != null)
                {
                    objaudit.PageAccessed = header.Referer.AbsoluteUri;
                }

                _auditCommand.InsertAuditData(objaudit);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}