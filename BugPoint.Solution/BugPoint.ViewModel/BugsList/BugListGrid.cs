using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.BugsList
{
    public class BugListGrid
    {
        public int RowNum { get; set; }
        public int BugId { get; set; }
        public string Summary { get; set; }
        public string ProjectName { get; set; }
        public string ComponentName { get; set; }
        public string PriorityName { get; set; }
        public string Resolution { get; set; }
        public string Severity { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string AssignedTo { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Reportedby { get; set; }
        public string ClosedOn { get; set; }

    }
}