using System.Collections.Generic;
using BugPoint.Model.Bugs;
using BugPoint.ViewModel.Bugs;

namespace BugPoint.ViewModel.BugsList
{
    public class DisplayBugViewModel
    {
        public BugDetailViewModel BugDetailViewModel { get; set; }
        public List<AttachmentsModel> ListofAttachments { get; set; }
        public BugReplyViewModel BugReplyViewModel { get; set; }
        public ViewBugReplyMainModel ViewBugReplyMainModel { get; set; }
        public ExpressChangesViewModel ExpressChangesViewModel { get; set; }

    }

    public class DisplayBugUserViewModel
    {
        public BugDetailViewModel BugDetailViewModel { get; set; }
        public List<AttachmentsModel> ListofAttachments { get; set; }
        public BugReplyUserViewModel BugReplyViewModel { get; set; }
        public ViewBugReplyMainModel ViewBugReplyMainModel { get; set; }
        public ExpressChangesUserViewModel ExpressChangesViewModel { get; set; }
    }
}