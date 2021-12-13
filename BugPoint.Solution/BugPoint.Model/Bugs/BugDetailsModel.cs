using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Bugs
{
    [Table("BugDetails")]
    public class BugDetailsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BugDetailsId { get; set; }

        [ForeignKey("BugSummary")]
        public long? BugSummaryId { get; set; }

        [MaxLength(999)]
        public string Description { get; set; }

        public long? BugId { get; set; }
    }
}