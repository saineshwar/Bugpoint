namespace BugPoint.Data.Bugs.Queries
{
    public interface IBugHistoryHelper
    {
        string CreateBug(string assignedto, string status, string priority);
        string ChangePriorityMessage(int? priorityId);
        string ChangeStatusMessage(int? statusId);
        string EditBugMessage(string user, long? bugId);
        string EditBugReplyMessage(string user, long? bugId);
        string DeleteReplyAttachmentMessage(string user, long? bugId);
        string DeleteAttachmentMessage(string user, long? bugId);
        string ReopenMessage(string user, long? bugId, string status);
        string ReplyMessage(string user, long? bugId, string status);
        string ClosedReplyMessage(string user, long? bugId, string status);
        string ReplyMessage(string user, long? bugId, string status, string resolution);
        string MovingBugsMessage(string fromuser, string touser);
    }
}