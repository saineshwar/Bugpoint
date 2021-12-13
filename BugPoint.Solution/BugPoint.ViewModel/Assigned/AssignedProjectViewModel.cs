using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.Assigned
{
    public class AssignedProjectViewModel
    {
        public int ProjectId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }

    public class RequestAssignedProjectViewModel
    {
        [MaxLength(50,ErrorMessage = "Maximum 50 charaters Allowed")]
        [Display(Name = "Project")]
        public string Project { get; set; }
        [Display(Name ="Role")]
        public int RoleId { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [MaxLength(30, ErrorMessage = "Maximum 30 charaters Allowed")]
        [Display(Name = "User")]
        public string User { get; set; }
    }

    public class ProjectAssignedUsersViewModel
    {
        public string Username { get; set; }
        public string UserId { get; set; }
    }
}