using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.Data.Masters.Queries
{
    public interface IMastersQueries
    {
        List<SelectListItem> ListofPriority();
        List<SelectListItem> ListofSeverity();
        List<SelectListItem> ListofResolution();
        List<SelectListItem> ListofStatus();
        List<SelectListItem> ListofUserStatus();
        List<SelectListItem> ListofReportStatus();
        List<SelectListItem> ListofHardware();
        List<SelectListItem> ListofOperatingSystem();
        List<SelectListItem> ListofVersions();
        List<SelectListItem> ListofBrowser();
        List<SelectListItem> ListofWebFramework();
        List<SelectListItem> ListofEnvironments();
        List<SelectListItem> ListofBugTypes();
        string GetPriorityNameBypriorityId(int? priorityId);
        string GetStatusBystatusId(int? statusId);
        string GetResolutionByResolutionId(int? resolutionId);
        List<SelectListItem> ListofDesignation();
    }
}