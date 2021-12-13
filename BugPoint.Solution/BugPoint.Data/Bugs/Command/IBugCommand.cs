using System.Collections.Generic;
using BugPoint.Model.Bugs;
using BugPoint.ViewModel.Bugs;

namespace BugPoint.Data.Bugs.Command
{
    public interface IBugCommand
    {
        bool AddBug(BugSummaryModel bugSummaryModel,
            BugDetailsModel bugDetailsModel,
            BugTrackingModel bugTrackingModel, List<BugAttachmentsViewModel> listofAttachment
        );

        bool UpdateBug(BugSummaryModel bugSummaryModel,
            BugDetailsModel bugDetailsModel,
            BugTrackingModel bugTrackingModel, List<BugAttachmentsViewModel> listofAttachment
        );

        bool DeleteAttachmentByAttachmentId(AttachmentsModel attachmentsModel,
            AttachmentDetailsModel attachmentDetailsModel);

        bool AddReply(BugReplyModel bugReplyModel,
            BugReplyDetailsModel bugReplyDetailsModel,
            List<BugAttachmentsViewModel> listofAttachment, BugTrackingModel bugTrackingModel);

        bool ChangeBugPriority(ChangePriorityRequestModel changePriorityRequestModel);

        bool ChangeBugAssignedUser(ChangeAssignedUserRequestModel changeAssignedUser);
        bool ChangeBugsAssignedTester(ChangeAssignedUserRequestModel changeAssignedUser);
        bool UpdateReply(BugReplyModel bugReplyModel, BugReplyDetailsModel bugReplyDetailsModel,
            List<BugAttachmentsViewModel> listofAttachment);

        bool DeleteReplyAttachmentByAttachmentId(ReplyAttachmentModel replyAttachment,
            ReplyAttachmentDetailsModel replyAttachmentDetails);

        bool UpdatebugStatus(long bugId, int statusId);
    }
}