using System;
using System.Data;
using BugPoint.Data.Bugs.Command;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.MovingBugs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BugPoint.Data.Assigned.Command
{
    public class MovingBugsCommand : IMovingBugsCommand
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MovingBugsCommand> _logger;
        public MovingBugsCommand(IConfiguration configuration, ILogger<MovingBugsCommand> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public bool MovingBugsProcess(MovingBugsResponse movingBugs, int? createdby)
        {
            try
            {
                var result = 0;
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                using SqlTransaction transaction = con.BeginTransaction();

                foreach (var bugs in movingBugs.ListofBugs)
                {
                    try
                    {
                        var param = new DynamicParameters();
                        param.Add("@BugId", bugs);
                        param.Add("@FromUserId", movingBugs.FromUserId);
                        param.Add("@ToUserId", movingBugs.ToUserId);
                        param.Add("@ProjectId", movingBugs.ProjectId);
                        param.Add("@CreatedBy", createdby);
                        param.Add("@RoleId", movingBugs.RoleId);
                        con.Execute("Usp_AddMovingBugsHistory", param, transaction, 0,
                            CommandType.StoredProcedure);
                        result = 1;
                    }
                    catch (Exception)
                    {
                        result = 0;
                        break;
                    }
                }

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BugCommand:ChangeBugPriority");
                throw;
            }
        }
    }
}