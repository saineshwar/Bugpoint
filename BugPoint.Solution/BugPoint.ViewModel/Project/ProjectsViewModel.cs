using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.Project
{
    public class ProjectsViewModel
    {
        [Display(Name = "Project Name")]
        [Required(ErrorMessage = "Enter Project Name")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Maximum 50 characters and Minimum 6")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Enter Project Description")]
        [Display(Name = "Project Description")]
        [MaxLength(100)]
        public string ProjectDescription { get; set; }

        public bool Status { get; set; }
    }
}