using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("DesignationMaster")]
    public class DesignationModel
    {
        [Key]
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

    }
}