using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Bugs
{
    [Table("BugHistory")]
    public class BugHistoryModel
    {
        [Key]
        public long BugHistoryId { get; set; }
        public string Message { get; set; }
        public DateTime? ProcessDate { get; set; }
        public long? UserId { get; set; }
        public long? BugId { get; set; }
        public long? StatusId { get; set; }
        public long? PriorityId { get; set; }
        public long? AssignedTo { get; set; }
        public DateTime? ClosedOn { get; set; }
    }
}