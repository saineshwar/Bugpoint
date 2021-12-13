using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Bugs
{
    [Table("BugReply")]
    public class BugReplyModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BugReplyId { get; set; }
        public long? BugId { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(20)]
        public string CreatedDateDisplay { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? DeleteStatus { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}