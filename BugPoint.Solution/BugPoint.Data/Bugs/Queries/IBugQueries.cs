using System.Collections.Generic;
using BugPoint.Model.Bugs;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.BugsList;

namespace BugPoint.Data.Bugs.Queries
{
    public interface IBugQueries
    {
        BugSummaryModel GetBugSummarybybugId(long? bugId);
        BugDetailsModel GetBugsDetailsbybugId(long? bugId);
        BugTrackingModel GetBugTrackingbybugId(long? bugId);
        List<AttachmentsModel> GetListAttachmentsBybugId(long? bugId);
        AttachmentsModel GetAttachmentsBybugId(long bugId, long attachmentId);
        AttachmentDetailsModel GetAttachmentDetailsByAttachmentId(long bugId, long attachmentId);
        bool AttachmentsExistbybugId(long? bugId);
        int GetReportersBugsCount(int? createdBy, int? projectId, int? projectComponentId, int? priorityId, int? severityId, int? statusId, int? assignedtoId);

        List<BugListGrid> GetReportersBugsList(int? createdBy, int? projectId, int? projectComponentId, int? priorityId, int? severityId, int? statusId, int? assignedtoId,
            int? pageNumber, int? pageSize);

        BugDetailViewModel GetBugDetailsbyBugId(long? bugId);
        List<ViewBugReplyHistoryModel> ListofHistoryTicketReplies(long? bugId);
        List<ReplyAttachmentModel> GetListReplyAttachmentsByAttachmentId(long? bugId, long? bugReplyId);
        ReplyAttachmentModel GetReplyAttachmentsBybugId(long bugId, long replyAttachmentId);
        ReplyAttachmentDetailsModel GetReplyAttachmentDetailsByAttachmentId(long bugId, long replyAttachmentId);
        EditBugReplyViewModel GetBugReplyEditDetailsbyBugId(long? bugId, long? bugReplyId);
        BugReplyDetailsModel GetBugReplyDetailsbyBugId(long? bugId, long? bugReplyId);
        BugReplyModel GetBugReplybyBugId(long? bugId, long? bugReplyId);
        bool ReplyAttachmentsExistbybugId(long? bugId, long? bugReplyId);

        List<BugHistoryViewModel> GetBugHistorybyBugId(long? bugId);

        int GetMyBugsCount(int? currentuser, int? projectId, int? projectComponentId, int? priorityId, int? severityId,
            int? statusId);
        List<BugListGrid> GetMyBugsList(int? currentuser, int? projectId, int? projectComponentId, int? priorityId,
            int? severityId, int? statusId, int? pageNumber, int? pageSize);

        int GetMyTeamsBugsCount(int? currentuser, int? projectId, int? projectComponentId, int? priorityId,
            int? severityId, int? statusId, int? devId);

        List<BugListGrid> GetMyTeamsBugsList(int? currentuser, int? projectId, int? projectComponentId, int? priorityId,
            int? severityId, int? statusId, int? devId, int? pageNumber, int? pageSize);

        int GetMyReportersBugsCount(int? createdBy, int? projectId, int? projectComponentId, int? priorityId,
            int? severityId, int? reportersUserId, int? statusId);

        List<BugListGrid> GetmyReportersBugsList(int? createdBy, int? projectId, int? projectComponentId,
            int? priorityId, int? severityId, int? statusId, int? reportersUserId, int? pageNumber,  int? pageSize);

        int GetManagerBugsCount(int? projectId,
            int? projectComponentId,
            int? priorityId,
            int? severityId,
            int? statusId,
            int? versionId,
            int? operatingSystemId,
            int? hardwareId,
            int? browserId,
            int? webFrameworkId,
            int? testedOnId,
            int? bugTypeId,
            int? reportersUserId,
            int? developersUserId,
            int? page);

        List<BugListGrid> GetManagerBugsList(int? projectId,
            int? projectComponentId,
            int? priorityId,
            int? severityId,
            int? statusId,
            int? versionId,
            int? operatingSystemId,
            int? hardwareId,
            int? browserId,
            int? webFrameworkId,
            int? testedOnId,
            int? bugTypeId,
            int? reportersUserId,
            int? developersUserId,
            int? pageNumber, int? pageSize);

        BugRecipientDetailsViewModel GetBugAssigntobybugId(long? bugId);
        BugRecipientDetailsViewModel GetBugTesterbybugId(long? bugId);


        int GetReportersBugsCountLastSevenDays(int? createdBy);

        List<BugListGrid> GetReportersBugsListLastSevenDays(int? createdBy,
            int? pageNumber, int? pageSize);

        int GetMyReportersBugsCountLastSevenDays(int? createdBy);

        List<BugListGrid> GetmyReportersBugsListLastSevenDays(int? createdBy, int? pageNumber, int? pageSize);


        int GetMyBugsCountLastSevenDays(int? currentuser);

        List<BugListGrid> GetMyBugsListLastSevenDays(int? currentuser, int? pageNumber, int? pageSize);

        int GetMyTeamsBugsCountLastSevenDays(int? currentuser);

        List<BugListGrid> GetMyTeamsBugsListLastSevenDays(int? currentuser, int? pageNumber, int? pageSize);

    }
}