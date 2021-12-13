using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.Reports
{
    public class BugDetailViewReportModel
    {
        public long? BugId { get; set; }
        public string Summary { get; set; }
        public string Project { get; set; }
        public string Component { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string ClosedOn { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Severity { get; set; }
        public string Resolution { get; set; }
        public string Hardware { get; set; }
        public string OperatingSystem { get; set; }
        public string WebFramework { get; set; }
        public string TestedOn { get; set; }
        public string Version { get; set; }
        public string BugType { get; set; }
        public string Browser { get; set; }
        public string Urls { get; set; }
        public string Description { get; set; }
    }
}