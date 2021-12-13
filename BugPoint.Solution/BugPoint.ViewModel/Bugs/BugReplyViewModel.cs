using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.Bugs
{
    public class BugReplyViewModel
    {
        public long? BugId { get; set; }

        [StringLength(700)]
        [Required(ErrorMessage = "Please Enter Description.")]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }
    }

    public class BugReplyUserViewModel
    {
        public long? BugId { get; set; }

        [StringLength(700)]
        [Required(ErrorMessage = "Please Enter Description.")]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }

        [Display(Name = "Resolution")]
        public int? ResolutionId { get; set; }
        public List<SelectListItem> ListofResolutions { get; set; }
    }
}
