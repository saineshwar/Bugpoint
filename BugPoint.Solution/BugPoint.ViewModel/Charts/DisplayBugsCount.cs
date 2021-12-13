namespace BugPoint.ViewModel.Charts
{
    public class DisplayBugsCount
    {
        public int OpenCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int InProgressCount { get; set; }
        public int ReOpenedCount { get; set; }
        public int ResolvedCount { get; set; }
        public int InTestingCount { get; set; }
        public int ClosedCount { get; set; }
        public int OnHoldCount { get; set; }
        public int RejectedCount { get; set; }
        public int ReplyCount { get; set; }
        public int DuplicateCount { get; set; }
        public int UnConfirmedCount { get; set; }
    }

    public class DisplayBugsReportCount
    {
        public string Names { get; set; }
        public int Open { get; set; }
        public int Confirmed { get; set; }
        public int InProgress { get; set; }
        public int ReOpened { get; set; }
        public int Resolved { get; set; }
        public int InTesting { get; set; }
        public int Closed { get; set; }
        public int OnHold { get; set; }
        public int Rejected { get; set; }
        public int Reply { get; set; }
        public int Duplicate { get; set; }
        public int UnConfirmed { get; set; }
        public int Total { get; set; }
    }

    public class DisplayStarPerformer
    {
        public string PerformerName { get; set; }
        public int TotalCount { get; set; }
    }



}