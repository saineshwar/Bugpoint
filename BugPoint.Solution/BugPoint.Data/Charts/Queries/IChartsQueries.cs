using System.Collections.Generic;
using BugPoint.ViewModel.Charts;

namespace BugPoint.Data.Charts.Queries
{
    public interface IChartsQueries
    {
        List<DisplayBugsCount> GetReporterStatusWiseBugCount(int? userId, int? projectId);
        List<ReporterCommonViewModel> GetReporterSeveritywiseCount(int? userId, int? projectId);
        List<ReporterCommonViewModel> GetBugTypeProjectwiseCount(int? userId, int? projectId);
        List<ReporterCommonViewModel> GetTestedEnvironmentProjectwiseCount(int? userId, int? projectId);
        List<ReporterCommonViewModel> GetDevelopersSeveritywiseCount(int? userId, int? projectId);
        List<ReporterCommonViewModel> GetDevelopersBugTypeProjectwiseCount(int? userId, int? projectId);
        List<ReporterCommonViewModel> GetDevelopersTestedEnvironmentProjectwiseCount(int? userId, int? projectId);
        List<ReporterStatusPieChartViewModel> GetReporterPieChartbyUserId(int? userId, int? projectId);
        List<ReporterPriorityPieChartViewModel> GetReporterPriorityPieChartbyUserId(int? userId, int? projectId);
        List<ReporterBugsCountPieChartViewModel> GetReporterBugsCountPieChartbyUserId(int? userId, int? projectId);
        List<ReporterProjectWiseBugsCountViewModel> GetReporterProjectWiseBugsCountbyUserId(int? userId);
        List<ReporterProjectWiseStatusBugsCountViewModel> GetReporterProjectWiseBugsCountbyUserId(int? userId, int? projectId);
        List<ReporterStatusPieChartViewModel> GetDeveloperPieChartbyUserId(int? userId, int? projectId);
        List<ReporterPriorityPieChartViewModel> GetDeveloperPriorityPieChartbyUserId(int? userId, int? projectId);
        List<ReporterBugsCountPieChartViewModel> GetDevelopersBugsCountPieChartbyUserId(int? userId, int? projectId);
        List<ReporterProjectWiseBugsCountViewModel> GetDeveloperProjectWiseBugsCountbyUserId(int? userId);
        List<DisplayBugsCount> GetDevelopersStatusWiseBugCount(int? userId, int? projectId);
        List<ReporterProjectWiseStatusBugsCountViewModel> GetDeveloperProjectWiseBugsCountbyUserId(int? userId, int? projectId);
        List<ReporterStatusPieChartViewModel> GetReporterLeadPieChartbyUserId(int? projectId);
        List<ReporterPriorityPieChartViewModel> GetReporterLeadPriorityPieChartbyUserId(int? projectId);
        List<ReporterBugsCountPieChartViewModel> GetReporterLeadBugsCountPieChartbyUserId(int? projectId);
        List<ReporterProjectWiseStatusBugsCountViewModel> GetReporterLeadProjectWiseBugsCountbyUserId(int? projectId);
        List<DisplayBugsCount> GetLeadStatusWiseBugCount(int? userId, int? projectId);
        List<ReporterCommonViewModel> GetLeadSeveritywiseCount(int? projectId);
        List<ReporterCommonViewModel> GetLeadBugTypeProjectwiseCount(int? projectId);
        List<ReporterCommonViewModel> GetLeadTestedEnvironmentProjectwiseCount(int? projectId);
        List<DisplayBugsReportCount> GetDeveloperTeamsBugsCount(int? projectId);
        List<DisplayBugsReportCount> GetTotalBugStatusCountProjectWise(int? projectId);
        List<DisplayBugsReportCount> GetTesterTeamsBugsCount(int? projectId);
        List<ReporterCommonViewModel> GetBrowserNamesofTestedBugs(int? projectId);
        List<ReporterCommonViewModel> GetHardwareDetails(int? projectId);
        List<ReporterCommonViewModel> GetVersionDetails(int? projectId);
        List<ReporterCommonViewModel> GetOperatingSystemDetails(int? projectId);
        List<ReporterCommonViewModel> GetWebFrameworkDetailsProjectWise(int? projectId);
        List<DisplayStarPerformer> GetStartTesterCount(int? projectId);
        List<DisplayStarPerformer> GetStartDeveloperCount(int? projectId);
    }
}