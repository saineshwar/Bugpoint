using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Bugs
{
    [Table("BugTracking")]
    public class BugTrackingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BugTrackingId { get; set; }
        public long? BugId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? AssignedTo { get; set; }
        public int? ResolutionId { get; set; }
        public DateTime? ClosedOn { get; set; }
    }

}