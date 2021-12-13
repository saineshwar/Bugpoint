using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace BugPoint.ViewModel.UserMaster
{
    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "UserName")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Maximum 30 characters and Minimum 6")]
        [Required(ErrorMessage = "Username Required")]
        public string UserName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Maximum 50 characters and Minimum 2")]
        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "Enter FirstName")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Maximum 50 characters and Minimum 2")]
        [Display(Name = "LastName")]
        [Required(ErrorMessage = "Enter LastName")]
        public string LastName { get; set; }

        [Display(Name = "EmailId")]
        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        public string EmailId { get; set; }

        [Display(Name = "MobileNo")]
        [Required(ErrorMessage = "Mobile-no Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobile-no")]
        public string MobileNo { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        [Display(Name = "Status")]
        public bool? Status { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Choose Role")]
        public int RoleId { get; set; }
        public List<SelectListItem> ListRole { get; set; }

        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Choose Designation")]
        public int DesignationId { get; set; }
        public List<SelectListItem> ListofDesignation { get; set; }
    }
}