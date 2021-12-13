using System.Data;
using System.Linq;
using BugPoint.Data.Bugs.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.Bugs.Queries
{
    public class BugNumberGeneratorQueries : IBugNumberGeneratorQueries
    {
        private readonly IConfiguration _configuration;
        public BugNumberGeneratorQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GenerateNo()
        {
            using var con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            con.Open();
            using var transcation = con.BeginTransaction();
            try
            {
                int value = 0;
                var para = new DynamicParameters();
                para.Add(name: "@RetVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                var returnCode = con.Execute("Usp_BugIdentity", para, transcation, commandType: CommandType.StoredProcedure);
                if (returnCode > 0)
                {
                    value = para.Get<int>("RetVal");
                    transcation.Commit();
                }
                else
                {
                    transcation.Rollback();
                }

                return value;
            }
            catch (System.Exception)
            {
                transcation.Rollback();
                throw;
            }

        }
    }
}