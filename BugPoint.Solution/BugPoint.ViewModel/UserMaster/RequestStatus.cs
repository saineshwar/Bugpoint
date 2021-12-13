using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.UserMaster
{
    public class RequestStatus
    {
        public int UserId { get; set; }
        public bool Status { get; set; }
    }

    public class RequestDevelopers
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string CustomName { get; set; }
    }

    public class TeamMembers
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string AssignedRole { get; set; }
        public string Gender { get; set; }
        public string AssignedProjectOn { get; set; }
    }

    public class TeamMembersModel
    {
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }
    }
}