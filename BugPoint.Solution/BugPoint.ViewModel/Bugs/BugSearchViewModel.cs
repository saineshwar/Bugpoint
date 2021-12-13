using System.ComponentModel.DataAnnotations;

namespace BugPoint.ViewModel.Bugs
{
    public class BugSearchViewModel
    {
        [Required(ErrorMessage = "BugId Required")]
        public int? BugIdSearch { get; set; }
    }
}