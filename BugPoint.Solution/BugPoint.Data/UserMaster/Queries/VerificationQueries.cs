using System.Data;
using System.Linq;
using BugPoint.ViewModel.Login;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.UserMaster.Queries
{
    public class VerificationQueries : IVerificationQueries
    {
        private readonly IConfiguration _configuration;
        public VerificationQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetResetGeneratedTokenbyUnq(int? unq)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@UserId", unq);
            var result = connection.Query<string>("USP_GetResetGeneratedToken", param, null, true, 0, CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }
    }
}