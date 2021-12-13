using System;
using System.Data;
using BugPoint.Data.EFContext;
using BugPoint.Model.Assigned;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.Assigned.Command
{
    public class ProjectComponentCommand : IProjectComponentCommand
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        public ProjectComponentCommand(BugPointContext bugPointContext, IConfiguration configuration)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
        }

        public int Add(ProjectComponentModel projectComponent)
        {
            _bugPointContext.ProjectComponentModel.Add(projectComponent);
            return _bugPointContext.SaveChanges();
        }

        public int Update(ProjectComponentModel projectComponent)
        {
            _bugPointContext.Entry(projectComponent).State = EntityState.Modified;
            return _bugPointContext.SaveChanges();
        }

        public bool Delete(int? projectComponentId)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@ProjectComponentId", projectComponentId);
                param.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                connection.Execute("Usp_DeleteProjectComponent", param, transaction, 0, CommandType.StoredProcedure);
                var result = param.Get<string>("@result");

                if (result == "success")
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
    }
}