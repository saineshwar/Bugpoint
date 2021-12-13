using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.RoleMaster
{
    public class EditRoleMasterViewModel
    {
        [Required(ErrorMessage = "Enter RoleName")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Choose Status")]
        public bool Status { get; set; }

        public int RoleId { get; set; }
    }
}