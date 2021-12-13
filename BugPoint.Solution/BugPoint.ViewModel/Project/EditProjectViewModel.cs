using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.Project
{
    public class EditProjectViewModel
    {
        public int ProjectId { get; set; }

        [Display(Name = "Project Name")]
        [Required(ErrorMessage = "Enter Project Name")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Maximum 30 characters and Minimum 6")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Enter Project Description")]
        [Display(Name = "Project Description")]
        [MaxLength(100)]
        public string ProjectDescription { get; set; }
        public bool Status { get; set; }
    }
}