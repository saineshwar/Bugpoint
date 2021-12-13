using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Bugs
{
    [Table("BugSummary")]
    public class BugSummaryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BugSummaryId { get; set; }
        [MaxLength(100)]
        public string Summary { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectComponentId { get; set; }
        public int? SeverityId { get; set; }
        public int? PriorityId { get; set; }
        public int? VersionId { get; set; }
        public int? OperatingSystemId { get; set; }
        public int? HardwareId { get; set; }
        public int? BrowserId { get; set; }
        public int? WebFrameworkId { get; set; }
        public int? TestedOnId { get; set; }
        public int? BugTypeId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        [MaxLength(100)]
        public string Urls { get; set; }

        public long? BugId { get; set; }


    }
}