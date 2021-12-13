using System;

namespace BugPoint.ViewModel.Assigned
{
    public class ProjectComponentGrid
    {
        public long ProjectComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentDescription { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Username { get; set; }
    }
}