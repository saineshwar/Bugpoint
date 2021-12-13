using System;
using System.Linq;
using BugPoint.Data.EFContext;
using BugPoint.Data.Masters.Queries;
using Microsoft.EntityFrameworkCore;

namespace BugPoint.Data.Bugs.Queries
{
    public class BugHistoryHelper : IBugHistoryHelper
    {
        private readonly IMastersQueries _mastersQueries;

        public BugHistoryHelper(IMastersQueries mastersQueries)
        {
            _mastersQueries = mastersQueries;
        }

        public string ChangePriorityMessage(int? priorityId)
        {
            string message = string.Empty;
            string priorityName = _mastersQueries.GetPriorityNameBypriorityId(priorityId);

            // Deleted
            if (priorityId != null)
            {
                message = $"Set priority as {priorityName}";
            }

            return message;
        }

        public string ChangeStatusMessage(int? statusId)
        {
            string message = string.Empty;
            string statusname = _mastersQueries.GetStatusBystatusId(statusId);

            // Deleted
            if (statusId != null)
            {
                message = $"Status Updated {statusname}";
            }

            return message;
        }

        public string CreateBug(string assignedto, string status, string priority)
        {
            
            var message = $"Created a new Bug. Ticker is Assigned to {assignedto},Status as {status}, priority as {priority}";
            return message;
        }

        public string EditBugMessage(string user, long? bugId)
        {
            string message = $"Bug #{bugId} Edited by {user}";
            return message;
        }

        public string EditBugReplyMessage(string user, long? bugId)
        {
            string message = $"Bug #{bugId} BugReply Edited by {user}";
            return message;
        }

        public string DeleteReplyAttachmentMessage(string user, long? bugId)
        {
            string message = $"Bug #{bugId} Deleted BugReply Attachment by {user}";
            return message;
        }

        public string DeleteAttachmentMessage(string user, long? bugId)
        {
            string message = $"Bug #{bugId} Deleted Attachment by {user}";
            return message;
        }

        public string ReplyMessage(string user, long? bugId , string status)
        {
            string message = $"Bug #{bugId} Repiled by {user},Status as {status}";
            return message;
        }
        public string ReopenMessage(string user, long? bugId, string status)
        {
            string message = $"Bug #{bugId} Reopened by {user},Status as {status}";
            return message;
        }

        public string ClosedReplyMessage(string user, long? bugId, string status)
        {
            string message = $"Bug #{bugId} Repiled by {user},Status as {status}";
            return message;
        }

        public string ReplyMessage(string user, long? bugId, string status, string resolution)
        {
            string message = $"Bug #{bugId} Repiled by {user},Status as {status}, Resolution as {resolution}";
            return message;
        }

        public string MovingBugsMessage(string fromuser, string touser)
        {
            string message = $"All Bugs of {fromuser} is assigned to {touser} on {DateTime.Now}";
            return message;
        }
    }
}