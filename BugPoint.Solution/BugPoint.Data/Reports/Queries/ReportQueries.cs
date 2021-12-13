using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BugPoint.ViewModel.Charts;
using BugPoint.ViewModel.Reports;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.Reports.Queries
{
    public class ReportQueries : IReportQueries
    {
        private readonly IConfiguration _configuration;
        public ReportQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Developer

        public List<BugsReportCountViewModel> GetDeveloperTeamsProjectwiseReport(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<BugsReportCountViewModel>("Usp_GetDeveloperTeamWiseStatusCount_Report", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BugsReportComponentWiseCountViewModel> GetDeveloperTeamsProjectwiseComponentReport(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<BugsReportComponentWiseCountViewModel>("Usp_GetDeveloperTeamWiseProjectComponent_Report", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BugReportDetailsExport> GetDeveloperBugOpenCloseDetailsReport(int? projectId, string fromdate, string todate)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                para.Add("@Fromdate", fromdate);
                para.Add("@Todate", todate);
                var data = con.Query<BugReportDetailsExport>("Usp_GetDeveloperBugOpenCloseDetails_Report", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BugDetailViewReportModel> GetBugDetailsbyCreatedDateReport(int? projectId, string fromdate, string todate)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                para.Add("@Fromdate", fromdate);
                para.Add("@Todate", todate);
                var data = con.Query<BugDetailViewReportModel>("Usp_GetBugDetailsbyCreatedDate_Report", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BugTimeTakenReportExport> GetTimeTakeReport(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<BugTimeTakenReportExport>("Usp_GetTimeTakeReport", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
      

        #endregion

        #region Tester

        public List<BugsReportTesterCountViewModel> GetTesterTeamsProjectwiseReport(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<BugsReportTesterCountViewModel>("Usp_GetTesterTeamWiseStatusCount_Report", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BugsReportComponentWiseCountViewModel> GetTesterTeamsProjectwiseComponentReport(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<BugsReportComponentWiseCountViewModel>("Usp_GetTesterTeamWiseProjectComponent_Report", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BugReportDetailsExport> GetTesterBugOpenCloseDetailsReport(int? projectId, string fromdate, string todate)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                para.Add("@Fromdate", fromdate);
                para.Add("@Todate", todate);
                var data = con.Query<BugReportDetailsExport>("Usp_GetTesterBugOpenCloseDetails_Report", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        public List<SelectListItem> ReportTypeList()
        {
            List<SelectListItem> listofReport = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "---Select---",Value = "",
                },
                new SelectListItem()
                {
                    Text = "Summary Report Project Wise",Value = "1",
                },
                new SelectListItem()
                {
                    Text = "Project Component Wise Report",Value = "2",
                },
                new SelectListItem()
                {
                    Text = "Project Open/Close Detail Report",Value = "3",
                },
                new SelectListItem()
                {
                    Text = "Project Detail Report",Value = "4",
                },
                new SelectListItem()
                {
                    Text = "Time Stats Report",Value = "5",
                }
            };
            return listofReport;
        }

        public List<SelectListItem> RoleTypeList()
        {
            List<SelectListItem> listofReport = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "---Select---",Value = "",
                },
                new SelectListItem()
                {
                    Text = "Developer",Value = "1",
                },
                new SelectListItem()
                {
                    Text = "Tester",Value = "2",
                }
            };
            return listofReport;
        }

    }
}