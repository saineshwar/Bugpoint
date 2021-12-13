namespace BugPoint.ViewModel.Reports
{
    public class BugsReportCountViewModel
    {
        public string Project { get; set; }
        public string Developer { get; set; }
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
    }

    public class BugsReportTesterCountViewModel
    {
        public string Project { get; set; }
        public string Tester { get; set; }
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
    }


    public class BugsReportComponentWiseCountViewModel
    {
        public string Project { get; set; }
        public string Component { get; set; }
        public string Developer { get; set; }
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
    }

    public class BugReportDetailsExport
    {
        public int BugId { get; set; }
        public string CreatedOn { get; set; }
        public string ClosedOn { get; set; }
        public string Createdby { get; set; }
        public string AssignedTo { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Resolution { get; set; }
    }

    public class BugTimeTakenReportExport
    {
        public string ProjectName { get; set; }
        public string Timetaken { get; set; }
        public string Createdby { get; set; }
        public string AssignedTo { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Resolution { get; set; }
        public string CreatedOn { get; set; }
        public string ClosedOn { get; set; }
    }



}