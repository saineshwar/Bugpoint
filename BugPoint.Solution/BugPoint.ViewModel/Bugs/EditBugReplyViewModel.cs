using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BugPoint.Model.Bugs;

namespace BugPoint.ViewModel.Bugs
{
    public class EditBugReplyViewModel
    {
        public long? BugId { get; set; }
        public long? BugReplyDetailsId { get; set; }
        public long? BugReplyId { get; set; }

        [StringLength(700)]
        [Required(ErrorMessage = "Please Enter Description.")]
        public string Description { get; set; }
        public List<ReplyAttachmentModel> ListofAttachments { get; set; }
    }
}