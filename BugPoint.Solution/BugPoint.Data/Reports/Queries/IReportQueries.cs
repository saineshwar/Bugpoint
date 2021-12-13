using System.Collections.Generic;
using BugPoint.ViewModel.Charts;
using BugPoint.ViewModel.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.Data.Reports.Queries
{
    public interface IReportQueries
    {
        List<BugsReportCountViewModel> GetDeveloperTeamsProjectwiseReport(int? projectId);
        List<BugsReportComponentWiseCountViewModel> GetDeveloperTeamsProjectwiseComponentReport(int? projectId);
        List<BugReportDetailsExport> GetDeveloperBugOpenCloseDetailsReport(int? projectId, string fromdate,
            string todate);
        List<SelectListItem> ReportTypeList();
        List<BugDetailViewReportModel> GetBugDetailsbyCreatedDateReport(int? projectId, string fromdate, string todate);
        List<SelectListItem> RoleTypeList();
        List<BugsReportTesterCountViewModel> GetTesterTeamsProjectwiseReport(int? projectId);
        List<BugsReportComponentWiseCountViewModel> GetTesterTeamsProjectwiseComponentReport(int? projectId);
        List<BugReportDetailsExport> GetTesterBugOpenCloseDetailsReport(int? projectId, string fromdate, string todate);
        List<BugTimeTakenReportExport> GetTimeTakeReport(int? projectId);
    }
}