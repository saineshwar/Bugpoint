using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Assigned
{
    [Table("ProjectComponent")]
    public class ProjectComponentModel
    {
        [Key]
        public long ProjectComponentId { get; set; }
        public int ProjectId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentDescription { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? AssignedTo { get; set; }
    }
}