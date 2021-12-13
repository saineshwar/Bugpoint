using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.Assigned
{
    public class ProjectComponentViewModel
    {
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int ProjectId { get; set; }

        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Component Name")]
        [Required(ErrorMessage = "Project Required")]
        public string ComponentName { get; set; }


        [Display(Name = "Description")]
        [Required(ErrorMessage = "Project Required")]
        [MaxLength(100)]
        public string ComponentDescription { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Assigned To")]
        [Required(ErrorMessage = "Assigned To is Required")]
        public int? AssignBugto { get; set; }
        public List<SelectListItem> ListofUsers { get; set; }
    }

    public class EditProjectComponentViewModel
    {
        public long ProjectComponentId { get; set; }

        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int ProjectId { get; set; }

        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Component Name")]
        [Required(ErrorMessage = "Project Required")]
        public string ComponentName { get; set; }


        [Display(Name = "Description")]
        [Required(ErrorMessage = "Project Required")]
        [MaxLength(100)]
        public string ComponentDescription { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Assigned To")]
        [Required(ErrorMessage = "Assigned To is Required")]
        public int? AssignBugto { get; set; }
        public List<SelectListItem> ListofUsers { get; set; }
    }


    public class RequestProjectComponentViewModel
    {
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }

        [Display(Name = "Component Name")]
        [Required(ErrorMessage = "Project Required")]
        public string ComponentName { get; set; }


        [Display(Name = "Description")]
        [Required(ErrorMessage = "Project Required")]
        public string ComponentDescription { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Assigned To")]
        [Required(ErrorMessage = "Assigned To is Required")]
        public int? AssignBugto { get; set; }
    }


}