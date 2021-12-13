using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.RoleMaster
{
    public class RoleMasterViewModel
    {
        [Required(ErrorMessage = "Enter RoleName")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Choose Status")]
        public bool Status { get; set; }
    }
}