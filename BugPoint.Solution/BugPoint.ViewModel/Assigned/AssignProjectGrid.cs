using System;

namespace BugPoint.ViewModel.Assigned
{
    public class AssignProjectGrid
    {
        public int AssignedProjectId { get; set; }
        public string ProjectName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }
        public string Designation { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int RoleId { get; set; }
    }
}