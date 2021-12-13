using System;
using System.Data;
using BugPoint.Data.EFContext;
using BugPoint.ViewModel.Login;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.UserMaster.Command
{
    public class VerificationCommand : IVerificationCommand
    {

        private readonly IConfiguration _configuration;
        public VerificationCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string InsertResetPasswordVerificationToken(ResetPasswordVerification resetPassword)
        {
            using SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlConnectionManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", resetPassword.UserId);
                param.Add("@GeneratedToken", resetPassword.GeneratedToken);
                param.Add("@GeneratedDate", resetPassword.GeneratedDate);

                var result = connection.Execute("USP_InsertResetPasswordVerificationToken", param, transaction, 0, CommandType.StoredProcedure);
                if (result > 0)
                {
                    sqlConnectionManager.Commit();
                    return "Success";
                }
                else
                {
                    sqlConnectionManager.Rollback();
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                sqlConnectionManager.Rollback();
                throw;
            }
        }

        public string UpdatePasswordandVerificationStatus(UpdateResetPasswordVerification resetPassword)
        {
            using SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlConnectionManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", resetPassword.UserId);
                param.Add("@GeneratedToken", resetPassword.GeneratedToken);
                param.Add("@Password", resetPassword.Password);

                var result = connection.Execute("USP_UpdatePasswordandVerificationStatus", param, transaction, 0, CommandType.StoredProcedure);
                if (result > 0)
                {
                    sqlConnectionManager.Commit();
                    return "Success";
                }
                else
                {
                    sqlConnectionManager.Rollback();
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                sqlConnectionManager.Rollback();
                throw;
            }
        }
    }
}