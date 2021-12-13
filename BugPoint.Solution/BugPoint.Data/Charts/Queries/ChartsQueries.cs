using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BugPoint.Data.EFContext;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.Charts;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.Charts.Queries
{
    public class ChartsQueries : IChartsQueries
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        public ChartsQueries(BugPointContext bugPointContext, IConfiguration configuration)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
        }

        #region Reporter
        public List<ReporterStatusPieChartViewModel> GetReporterPieChartbyUserId(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterStatusPieChartViewModel>("Usp_ReporterPieChartData", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<ReporterPriorityPieChartViewModel> GetReporterPriorityPieChartbyUserId(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                var data = con.Query<ReporterPriorityPieChartViewModel>("Usp_ReporterPriorityPieChartData", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<ReporterBugsCountPieChartViewModel> GetReporterBugsCountPieChartbyUserId(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterBugsCountPieChartViewModel>("Usp_GetOpenandClosedCountbyUserId", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<ReporterProjectWiseBugsCountViewModel> GetReporterProjectWiseBugsCountbyUserId(int? userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                var data = con.Query<ReporterProjectWiseBugsCountViewModel>("Usp_GetBugsCountProjectwisebyUserId", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<ReporterProjectWiseStatusBugsCountViewModel> GetReporterProjectWiseBugsCountbyUserId(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterProjectWiseStatusBugsCountViewModel>("Usp_GetBugsProjectwiseCountbyProjectId", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DisplayBugsCount> GetReporterStatusWiseBugCount(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@CreatedBy", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<DisplayBugsCount>("Usp_GetStatusWiseBugCount_Reporter", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetReporterSeveritywiseCount(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetSeverityProjectwiseCount_Reporter", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetBugTypeProjectwiseCount(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetBugTypeProjectwiseCount_Reporter", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetTestedEnvironmentProjectwiseCount(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetTestedEnvironmentProjectwiseCount_Reporter", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion


        #region Developer

        public List<ReporterStatusPieChartViewModel> GetDeveloperPieChartbyUserId(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterStatusPieChartViewModel>("Usp_DeveloperPieChartData", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterPriorityPieChartViewModel> GetDeveloperPriorityPieChartbyUserId(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterPriorityPieChartViewModel>("Usp_DeveloperPriorityPieChartData", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterBugsCountPieChartViewModel> GetDevelopersBugsCountPieChartbyUserId(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterBugsCountPieChartViewModel>("Usp_GetOpenandClosedDevelopersBugsCountbyUserId", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterProjectWiseBugsCountViewModel> GetDeveloperProjectWiseBugsCountbyUserId(int? userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                var data = con.Query<ReporterProjectWiseBugsCountViewModel>("Usp_GetDevelopersBugsCountProjectwisebyUserId", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterProjectWiseStatusBugsCountViewModel> GetDeveloperProjectWiseBugsCountbyUserId(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterProjectWiseStatusBugsCountViewModel>("Usp_GetDeveloperBugsProjectwiseCountbyProjectId", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DisplayBugsCount> GetDevelopersStatusWiseBugCount(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@AssignedTo", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<DisplayBugsCount>("Usp_GetStatusWiseBugCount_Developers", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetDevelopersSeveritywiseCount(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetSeverityProjectwiseCount_Developer", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetDevelopersBugTypeProjectwiseCount(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetBugTypeProjectwiseCount_Developer", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetDevelopersTestedEnvironmentProjectwiseCount(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetTestedEnvironmentProjectwiseCount_Developer", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }




        #endregion

        #region Reporter TeamLeads
        public List<ReporterStatusPieChartViewModel> GetReporterLeadPieChartbyUserId(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterStatusPieChartViewModel>("Usp_ReporterLeadPieChartData", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterPriorityPieChartViewModel> GetReporterLeadPriorityPieChartbyUserId(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterPriorityPieChartViewModel>("Usp_ReporterTeamLeadPriorityPieChartData", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterBugsCountPieChartViewModel> GetReporterLeadBugsCountPieChartbyUserId(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterBugsCountPieChartViewModel>("Usp_GetReporterTeamLeadOpenandClosedCount", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterProjectWiseStatusBugsCountViewModel> GetReporterLeadProjectWiseBugsCountbyUserId(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterProjectWiseStatusBugsCountViewModel>("Usp_GetReporterTeamLeadBugsProjectwiseCount", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DisplayBugsCount> GetLeadStatusWiseBugCount(int? userId, int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<DisplayBugsCount>("Usp_GetStatusWiseBugCount_Lead", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetLeadSeveritywiseCount(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
             
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetSeverityProjectwiseCount_Lead", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetLeadBugTypeProjectwiseCount( int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
         
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetBugTypeProjectwiseCount_Lead", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetLeadTestedEnvironmentProjectwiseCount(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
               
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetTestedEnvironmentProjectwiseCount_Lead", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DisplayBugsReportCount> GetDeveloperTeamsBugsCount(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<DisplayBugsReportCount>("Usp_GetDeveloperTeamWiseStatusCount_Lead", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DisplayBugsReportCount> GetTotalBugStatusCountProjectWise(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<DisplayBugsReportCount>("Usp_GetTotalBugStatusCountProjectWise", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<DisplayBugsReportCount> GetTesterTeamsBugsCount(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<DisplayBugsReportCount>("Usp_GetTesterTeamWiseStatusCount_Lead", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ReporterCommonViewModel> GetBrowserNamesofTestedBugs(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetBrowserNamesofTestedBugs", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetHardwareDetails(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetHardwareDetailsProjectWise", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<ReporterCommonViewModel> GetVersionDetails(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetVersionDetailsProjectWise", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetOperatingSystemDetails(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetOperatingSystemDetailsProjectWise", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReporterCommonViewModel> GetWebFrameworkDetailsProjectWise(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<ReporterCommonViewModel>("Usp_GetWebFrameworkDetailsProjectWise", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Stars of Bugpoint

        public List<DisplayStarPerformer> GetStartTesterCount(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<DisplayStarPerformer>("Usp_GetStarTester", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<DisplayStarPerformer> GetStartDeveloperCount(int? projectId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@ProjectId", projectId);
                var data = con.Query<DisplayStarPerformer>("Usp_GetStarDeveloper", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion



    }
}