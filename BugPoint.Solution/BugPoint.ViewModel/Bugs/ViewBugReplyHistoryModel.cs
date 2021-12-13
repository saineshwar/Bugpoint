using System;
using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.Bugs
{
    public class ViewBugReplyHistoryModel
    {
        public long BugId { get; set; }
        public long BugReplyId { get; set; }
        public string CreatedDateDisplay { get; set; }
        public string RepliedUserName { get; set; }

        [StringLength(700)]
        [Required(ErrorMessage = "Please Enter Description.")]
        public string Description { get; set; }
        public string Viewcolor { get; set; }
        public string EditOption { get; set; }
        public DateTime CreatedOn { get; set; }
        public int RoleId { get; set; }
    }
}