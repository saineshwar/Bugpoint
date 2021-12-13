namespace BugPoint.ViewModel.Assigned
{
    public class RequestRemoveAssignedProject
    {
        public int? AssignedProjectId { get; set; }
        public string AccessType { get; set; }
    }

    public class RequestDeleteComponent
    {
        public int? ProjectComponentId { get; set; }
    }

    public class RequestDeleteAssignedProject
    {
        public int? AssignedProjectId { get; set; }
        public int? RoleId { get; set; }
    }

}