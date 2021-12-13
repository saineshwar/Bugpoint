using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BugPoint.ViewModel.Assigned;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace BugPoint.ViewModel.BugsList
{
    public class BugListViewModel
    {
        public int? PageSize;

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }

        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Project Component")]
        public int? ProjectComponentId { get; set; }
        public List<SelectListItem> ListofComponents { get; set; }

        [Display(Name = "Severity")]
        public int? SeverityId { get; set; }

        public List<SelectListItem> ListofSeverity { get; set; }

        [Display(Name = "Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [Display(Name = "AssignedTo")]
        public int? AssignedtoId { get; set; }
        public List<SelectListItem> ListofDeveloperandLead { get; set; }

        public StaticPagedList<BugListGrid> BugListGrid { get; set; }
    }

    public class ReportedBugListViewModel
    {
        public int? PageSize;

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }

        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Project Component")]
        public int? ProjectComponentId { get; set; }
        public List<SelectListItem> ListofComponents { get; set; }

        [Display(Name = "Severity")]
        public int? SeverityId { get; set; }

        public List<SelectListItem> ListofSeverity { get; set; }

        [Display(Name = "Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        public StaticPagedList<BugListGrid> BugListGrid { get; set; }


        [Display(Name = "Reporters")]
        public int? ReportersUserId { get; set; }

        public List<SelectListItem> ListofReporters { get; set; }
    }

    public class DeveloperBugListViewModel
    {
        public int? PageSize;

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }

        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Project Component")]
        public int? ProjectComponentId { get; set; }
        public List<SelectListItem> ListofComponents { get; set; }

        [Display(Name = "Severity")]
        public int? SeverityId { get; set; }

        public List<SelectListItem> ListofSeverity { get; set; }

        [Display(Name = "Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        public StaticPagedList<BugListGrid> BugListGrid { get; set; }


        [Display(Name = "Developers")]
        public int? DevId { get; set; }
        public List<SelectListItem> ListofDevelopers { get; set; }
    }

    public class AllBugListViewModel
    {
        public int? PageSize;

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }

        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Project Component")]
        public int? ProjectComponentId { get; set; }
        public List<SelectListItem> ListofComponents { get; set; }

        [Display(Name = "Severity")]
        public int? SeverityId { get; set; }

        public List<SelectListItem> ListofSeverity { get; set; }

        [Display(Name = "Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        public StaticPagedList<BugListGrid> BugListGrid { get; set; }

        [Display(Name = "Version")]
        public int? VersionId { get; set; }
        public List<SelectListItem> ListofVersion { get; set; }

        [Display(Name = "OS")]
        public int? OperatingSystemId { get; set; }

        public List<SelectListItem> ListofOperatingSystem { get; set; }

        [Display(Name = "Hardware")]
        public int? HardwareId { get; set; }

        public List<SelectListItem> ListofHardware { get; set; }

        [Display(Name = "Browser Used")]
        public int? BrowserId { get; set; }

        public List<SelectListItem> ListofBrowser { get; set; }

        [Display(Name = "WebFramework")]
        public int? WebFrameworkId { get; set; }

        public List<SelectListItem> ListofWebFramework { get; set; }

        [Display(Name = "TestedOn")]
        public int? TestedOnId { get; set; }

        public List<SelectListItem> ListofTestedOn { get; set; }

        [Display(Name = "Bug Type")]
        public int? BugTypeId { get; set; }
        public List<SelectListItem> ListofBugType { get; set; }

        [Display(Name = "Reporters")]
        public int? ReportersUserId { get; set; }

        public List<SelectListItem> ListofReporters { get; set; }

        [Display(Name = "Assigned To")]
        public int? DevelopersUserId { get; set; }
        public List<SelectListItem> ListofDevelopers { get; set; }
    }

    public class AllBugsFilterParameters
    {
        public int? StatusId { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectComponentId { get; set; }
        public int? SeverityId { get; set; }
        public int? PriorityId { get; set; }
        public int? VersionId { get; set; }
        public int? OperatingSystemId { get; set; }
        public int? HardwareId { get; set; }
        public int? BrowserId { get; set; }
        public int? WebFrameworkId { get; set; }
        public int? TestedOnId { get; set; }
        public int? BugTypeId { get; set; }
        public int? ReportersUserId { get; set; }
        public int? DevelopersUserId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }

    public class ReportedRecentBugListViewModel
    {
        public int? Page;

        public StaticPagedList<BugListGrid> BugListGrid { get; set; }
    }
}