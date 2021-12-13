using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Bugs
{
    [Table("BugReplyDetails")]
    public class BugReplyDetailsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BugReplyDetailsId { get; set; }
        public long? BugId { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public long? BugReplyId { get; set; }
    }

}