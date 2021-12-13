using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.Bugs
{
    public class BugViewModel
    {
        [Display(Name = "Bug Title")]
        [Required(ErrorMessage = "Bug Title Required")]
        public string Summary { get; set; }

        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Project Component")]
        [Required(ErrorMessage = "Project Component Required")]
        public int? ProjectComponentId { get; set; }
        public List<SelectListItem> ListofComponents { get; set; }

        [Display(Name = "Severity")]
        [Required(ErrorMessage = "Severity Required")]
        public int? SeverityId { get; set; }

        public List<SelectListItem> ListofSeverity { get; set; }

        [Display(Name = "Priority")]
        [Required(ErrorMessage = "Priority Required")]
        public int? PriorityId { get; set; }

        public List<SelectListItem> ListofPriority { get; set; }

        [Display(Name = "Version")]
        [Required(ErrorMessage = "Version Required")]
        public int? VersionId { get; set; }
        public List<SelectListItem> ListofVersion { get; set; }

        [Display(Name = "OS")]
        [Required(ErrorMessage = "OS Required")]
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

        [Display(Name = "Bug Description")]
        [Required(ErrorMessage = "Bug Description Required")]
        public string Description { get; set; }

        [Display(Name = "URL")]
        [Required(ErrorMessage = "URL")]
        public string Urls { get; set; }

        [Display(Name = "Assigned To")]
        [Required(ErrorMessage = "Assigned To is Required")]
        public int? AssignBugto { get; set; }
        public List<SelectListItem> ListofUsers { get; set; }

    }
}