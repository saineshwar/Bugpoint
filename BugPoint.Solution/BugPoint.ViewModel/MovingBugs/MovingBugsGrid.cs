namespace BugPoint.ViewModel.MovingBugs
{
    public class MovingBugsGrid
    {
        public long? BugId { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public string Summary { get; set; }
        public int? AssignedTo { get; set; }
        public int? CreatedBy { get; set; }
    }
}