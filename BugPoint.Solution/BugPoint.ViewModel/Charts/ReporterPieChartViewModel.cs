using System.Collections.Generic;

namespace BugPoint.ViewModel.Charts
{
    public class ReporterStatusPieChartViewModel
    {
        public int TotalCount { get; set; }
        public string StatusName { get; set; }
    }

    public class ReporterPriorityPieChartViewModel
    {
        public int TotalCount { get; set; }
        public string PriorityName { get; set; }
    }

    public class ReporterBugsCountPieChartViewModel
    {
        public int Open { get; set; } = 0;
        public int Closed { get; set; } = 0;
    }

    public class ReporterProjectWiseBugsCountViewModel
    {
        public int Open { get; set; } = 0;
        public int Closed { get; set; } = 0;
        public string ProjectName { get; set; }
    }

    public class ReporterProjectWiseStatusBugsCountViewModel
    {
        public int TotalCount { get; set; } = 0;
        public string StatusName { get; set; }
    }

    public class ReporterCommonViewModel
    {
        public int TotalCount { get; set; } = 0;
        public string TextValue { get; set; }
    }

    public class ReporterCommonMainViewModel
    {
        public List<ReporterCommonViewModel> ReporterCommonViewModel { get; set; }
        public string Heading { get; set; }
    }


    public class ReporterCommonPieChartViewModel
    {
        public int TotalCount { get; set; }
        public string TextValue { get; set; }
    }
}