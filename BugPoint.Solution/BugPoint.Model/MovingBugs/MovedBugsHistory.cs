using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.MovingBugs
{
    [Table("MovedBugsHistory")]
    public class MovedBugsHistory
    {
        [Key]
        public long MovedbugId { get; set; }
        public long BugId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int ProjectId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}