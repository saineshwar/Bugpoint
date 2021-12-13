using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.Bugs
{
    public class ChangePriorityRequestModel
    {
        [Required(ErrorMessage = "Required BugId")]
        public long BugId { get; set; }
        [Required(ErrorMessage = "Required PriorityId")]
        public short PriorityId { get; set; }
    }

    public class ChangeAssignedUserRequestModel
    {
        [Required(ErrorMessage = "Required TicketId")]
        public long BugId { get; set; }
        [Required(ErrorMessage = "Required CategoryId")]
        public short UserId { get; set; }
    }

    public class ChangeStatusRequestModel
    {
        [Required(ErrorMessage = "Required BugId")]
        public long BugId { get; set; }
    }


    public class ChangeAssignedDeveloperRequestModel
    {
        public int? ProjectId { get; set; }
        public string Username { get; set; }
    }

    public class BugMovementRequestModel
    {
        [Required(ErrorMessage = "Required User")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Required Role")]
        public int? RoleId { get; set; }

        [Required(ErrorMessage = "Required Username")]
        public string Username { get; set; }
    }

}