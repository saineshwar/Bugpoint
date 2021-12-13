using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BugPoint.Model.UserMaster;

namespace BugPoint.Model.AssignedRoles
{
    [Table("SavedAssignedRoles")]
    public class AssignedRolesModel
    {
        [Key]
        public int AssignedRoleId { get; set; }
        public int RoleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
        [ForeignKey("UserMaster")]
        public int? UserId { get; set; }
        public UserMasterModel UserMaster { get; set; }
    }
}