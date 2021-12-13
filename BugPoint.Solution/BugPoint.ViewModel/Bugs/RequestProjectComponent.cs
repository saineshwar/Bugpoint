namespace BugPoint.ViewModel.Bugs
{
    public class RequestProjectComponent
    {
        public int ProjectId { get; set; }
    }

    public class RequestProjectComponentDesc
    {
        public int ProjectComponentId { get; set; }
    }

    public class RequestBugActivities
    {
        public int BugId { get; set; }
    }

    public class RequestBugReopen
    {
        public int BugId { get; set; }
    }

    public class RequestProjectReporter
    {
        public int ProjectId { get; set; }
    }
}