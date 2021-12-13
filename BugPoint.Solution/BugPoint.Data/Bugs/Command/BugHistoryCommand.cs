using System;
using System.Data;
using BugPoint.Data.EFContext;
using BugPoint.Model.Bugs;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.Bugs.Command
{
    public class BugHistoryCommand : IBugHistoryCommand
    {
        private readonly IConfiguration _configuration;
        public BugHistoryCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool InsertBugHistory(BugHistoryModel bugHistory)
        {
            try
            {
                using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
                try
                {

                    var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                    var param = new DynamicParameters();
                    param.Add("@MESSAGE", bugHistory.Message);
                    param.Add("@ProcessDate", bugHistory.ProcessDate);
                    param.Add("@UserId", bugHistory.UserId);
                    param.Add("@BugId", bugHistory.BugId);
                    param.Add("@StatusId", bugHistory.StatusId);
                    param.Add("@PriorityId", bugHistory.PriorityId);
                    param.Add("@AssignedTo", bugHistory.AssignedTo);

                    var result = connection.Execute("Usp_InsertBugHistory", param, transaction, 0, CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        sqlDataAccessManager.Commit();
                        return true;
                    }
                    else
                    {
                        sqlDataAccessManager.Rollback();
                        return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}