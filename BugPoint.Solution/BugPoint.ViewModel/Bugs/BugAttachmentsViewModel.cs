using System;
using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.Bugs
{
    public class BugAttachmentsViewModel
    {

        public string OriginalAttachmentName { get; set; }
        public string GenerateAttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public long? BugId { get; set; }
        public string AttachmentBase64 { get; set; }
        public string BucketName { get; set; }
        public string DirectoryName { get; set; }
    }

    public class BugReplyAttachmentsViewModel
    {

        [MaxLength(100)]
        public string AttachmentName { get; set; }
        [MaxLength(100)]
        public string AttachmentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public long? BugId { get; set; }
        public string AttachmentBase64 { get; set; }
    }
}