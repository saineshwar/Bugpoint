using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.Reports
{
    public class ExportReportViewModel
    {
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Report Type")]
        [Required(ErrorMessage = "Report Type Required")]
        public int? ReportsId { get; set; }
        public List<SelectListItem> ListofReportType { get; set; }

        [Display(Name = "From date")]
        public string Fromdate { get; set; }

        [Display(Name = "To date")]
        public string Todate { get; set; }
    }

    public class ExportReportManagerViewModel
    {
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Role Required")]
        public int? RoleId { get; set; }
        public List<SelectListItem> TypeofRole { get; set; }

        [Display(Name = "Report Type")]
        [Required(ErrorMessage = "Report Type Required")]
        public int? ReportsId { get; set; }
        public List<SelectListItem> ListofReportType { get; set; }

        [Display(Name = "From date")]
        public string Fromdate { get; set; }

        [Display(Name = "To date")]
        public string Todate { get; set; }
    }
}