using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Project
{
    [Table("Projects")]
    public class ProjectsModel
    {
        [Key]
        public int ProjectId { get; set; }
        [MaxLength(50)]
        public string ProjectName { get; set; }
        [MaxLength(100)]
        public string ProjectDescription { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Status { get; set; }

    }
}