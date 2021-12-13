using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.MovingBugs
{
    public class MovingBugsViewModel
    {
        [Display(Name = "Project")]
        [MaxLength(50, ErrorMessage = "Maximum 50 charaters Allowed")]
        public string Project { get; set; }

        [Required(ErrorMessage = "Role Required")]
        public int RoleId { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [Display(Name = "From")]
        [MaxLength(30, ErrorMessage = "Maximum 30 charaters Allowed")]
        public string FromUser { get; set; }

        [Display(Name = "To")]
        [MaxLength(30, ErrorMessage = "Maximum 30 charaters Allowed")]
        public string ToUser { get; set; }
    }

    public class MovingBugsResponse
    {
        public int? FromUserId { get; set; }
        public int? ProjectId { get; set; }
        public int? ToUserId { get; set; }
        public int? RoleId { get; set; }
        public List<int> ListofBugs { get; set; }
    }

}