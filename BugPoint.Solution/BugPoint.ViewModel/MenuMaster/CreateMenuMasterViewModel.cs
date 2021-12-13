using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.MenuMaster
{
    public class CreateMenuMasterViewModel
    {
        [Display(Name = "Area")]
        public string Area { get; set; }

        [Display(Name = "ControllerName")]
        [Required(ErrorMessage = "Enter Controller Name")]
        public string ControllerName { get; set; }


        [Display(Name = "ActionMethod")]
        [Required(ErrorMessage = "Enter Action Method")]
        public string ActionMethod { get; set; }

        [Display(Name = "MenuName")]
        [Required(ErrorMessage = "Enter Menu Name")]
        public string MenuName { get; set; }
        public bool Status { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Required Role")]
        public int RoleId { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [Display(Name = "MenuCategory")]
        [Required(ErrorMessage = "Required Menu Category")]
        public int MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
    }
}