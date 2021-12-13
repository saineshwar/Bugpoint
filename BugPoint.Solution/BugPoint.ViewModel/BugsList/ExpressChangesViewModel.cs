using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.BugsList
{
    public class ExpressChangesViewModel
    {

        [Display(Name = "Priority")]
        [Required(ErrorMessage = "Priority Required")]
        public int? PriorityId { get; set; }

        public List<SelectListItem> ListofPriority { get; set; }

        [Display(Name = "Assigned To")]
        [Required(ErrorMessage = "Assigned To is Required")]
        public int? AssignedToId { get; set; }
        public List<SelectListItem> ListofUsers { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }

        [Display(Name = "Priority")]
        public string Priority { get; set; }

        [Display(Name = "Testers")]
        public int? TesterId { get; set; }
        public List<SelectListItem> ListofTesters { get; set; }

    }

    public class ExpressChangesUserViewModel
    {
        public int? RequestedUserId { get; set; }

        [Display(Name = "Assigned to Developer")]
        public string RequestforSolution { get; set; }
    }
}