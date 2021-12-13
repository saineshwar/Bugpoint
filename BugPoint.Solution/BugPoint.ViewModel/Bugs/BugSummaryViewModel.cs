using System;
using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.Bugs
{
    public class BugDetailViewModel
    {
        public long? BugId { get; set; }
        [Display(Name = "Bug Title")]
        public string Summary { get; set; }

        [Display(Name = "Project")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Component")]
        public string ProjectComponent { get; set; }
        [Display(Name = "Severity")]
        public string Severity { get; set; }
        [Display(Name = "Priority")]
        public string Priority { get; set; }
        [Display(Name = "Version")]
        public string Version { get; set; }
        [Display(Name = "OS")]
        public string OperatingSystemName { get; set; }
        [Display(Name = "Hardware")]
        public string Hardware { get; set; }
        [Display(Name = "Browser Used")]
        public string BrowserName { get; set; }
        [Display(Name = "WebFramework")]
        public string WebFramework { get; set; }
        [Display(Name = "TestedOn")]
        public string TestedOn { get; set; }

        [Display(Name = "Bug Type")]
        public string BugTypeOn { get; set; }
        [Display(Name = "URL")]
        public string Urls { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        public long BugDetailsId { get; set; }

        public int StatusId { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "CreatedOn")]
        public string CreatedOn { get; set; }

        [Display(Name = "ModifiedOn")]
        public string ModifiedOn { get; set; }

        public int? AssignedToId { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }


        [Display(Name = "Resolution")]
        public string Resolution { get; set; }

        public int? TesterId { get; set; }

        [Display(Name = "Reportedby")]
        public string CreatedBy { get; set; }

        public int? ProjectId { get; set; }

        public int? PriorityId { get; set; }
        public int? DesignationId { get; set; }
    }
}