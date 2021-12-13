using System;

namespace BugPoint.ViewModel.Project
{
    public class ProjectGrid
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}