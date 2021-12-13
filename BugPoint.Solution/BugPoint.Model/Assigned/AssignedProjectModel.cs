using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Assigned
{
    [Table("AssignedProject")]
    public class AssignedProjectModel
    {
        [Key]
        public int AssignedProjectId { get; set; }
        public int ProjectId { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? UserId { get; set; }
        
    }
}