using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.MenuMaster
{
    public class EditMenuMasterViewModel
    {

        public string Area { get; set; }
        public int MenuId { get; set; }

        [Required(ErrorMessage = "Enter Controller Name")]
        public string ControllerName { get; set; }

        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }

        [Required(ErrorMessage = "Enter Menu Name")]
        public string MenuName { get; set; }
        public bool Status { get; set; }

        [Required(ErrorMessage = "Required Role")]
        public int? RoleId { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [Required(ErrorMessage = "Required Menu Category")]
        public int? MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
    }
}