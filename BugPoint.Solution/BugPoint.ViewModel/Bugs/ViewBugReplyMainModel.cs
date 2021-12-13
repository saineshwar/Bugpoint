using System.Collections.Generic;
using BugPoint.Model.Bugs;

namespace BugPoint.ViewModel.Bugs
{
    public class ViewBugReplyMainModel
    {
        public List<ViewBugReplyHistoryModel> ListofTicketreply { get; set; }
        public List<ReplyAttachmentModel> ListofReplyAttachment { get; set; }
    }
}