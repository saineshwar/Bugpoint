using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Bugs
{
    [Table("ReplyAttachment")]
    public class ReplyAttachmentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplyAttachmentId { get; set; }
        public string OriginalAttachmentName { get; set; }
        public string GenerateAttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public long? BugId { get; set; }
        public long? BugReplyId { get; set; }
        public string BucketName { get; set; }
        public string DirectoryName { get; set; }
    }

    [Table("ReplyAttachmentDetails")]
    public class ReplyAttachmentDetailsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplyAttachmentDetailsId { get; set; }
        [MaxLength]
        public string AttachmentBase64 { get; set; }
        public long? BugId { get; set; }
        public long ReplyAttachmentId { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }

}