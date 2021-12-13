using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.ViewModel.Charts
{
    public class ReportChartTableViewModel
    {
        [Display(Name = "Project")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Required(ErrorMessage = "BugId Required")]
        public int? BugIdSearch { get; set; }
    }

    public class ReportTeamLeadChartTableViewModel
    {
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Assignto")]
        public int? Assignto { get; set; }
        public List<SelectListItem> ListofDevelopersandTeamLead { get; set; }

        [Required(ErrorMessage = "BugId Required")]
        public int? BugIdSearch { get; set; }
    }

    public class DeveloperTeamLeadChartTableViewModel
    {
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }
        public List<SelectListItem> ListofProjects { get; set; }

        [Display(Name = "Assignto")]
        public int? Assignto { get; set; }
        public List<SelectListItem> ListofDevelopersandTeamLead { get; set; }

        [Required(ErrorMessage = "BugId Required")]
        public int? BugIdSearch { get; set; }
    }


    public class ReportManagerChartTableViewModel
    {
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project Required")]
        public int? ProjectId { get; set; }

        public List<SelectListItem> ListofProjects { get; set; }

        [Required(ErrorMessage = "BugId Required")]
        public int? BugIdSearch { get; set; }

    }


    public class PieDataset
    {
        public string label { get; set; }
        public List<string> backgroundColor { get; set; }
        public int borderWidth { get; set; }
        public List<int> data { get; set; }
    }

    public class PieRoot
    {
        public List<string> labels { get; set; }
        public List<PieDataset> datasets { get; set; }
    }

    public class BarChartDataset
    {
        public string label { get; set; }
        public int barPercentage { get; set; }
        public int barThickness { get; set; }
        public int maxBarThickness { get; set; }
        public int minBarLength { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public bool pointRadius { get; set; }
        public string pointColor { get; set; }
        public string pointStrokeColor { get; set; }
        public string pointHighlightFill { get; set; }
        public string pointHighlightStroke { get; set; }
        public List<int> data { get; set; }
    }

    public class BarChartRoot
    {
        public List<string> labels { get; set; }
        public List<BarChartDataset> datasets { get; set; }
    }



}