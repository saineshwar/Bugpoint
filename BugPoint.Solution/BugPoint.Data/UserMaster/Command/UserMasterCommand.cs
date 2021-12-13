using System;
using System.Data;
using BugPoint.Data.EFContext;
using BugPoint.Model.AssignedRoles;
using BugPoint.Model.UserMaster;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.UserMaster.Command
{
    public class UserMasterCommand : IUserMasterCommand
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        public UserMasterCommand(BugPointContext bugPointContext, IConfiguration configuration)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
        }

        public long? AddUser(UserMasterModel usermaster, int? roleId)
        {
            try
            {
                using var dbContextTransaction = _bugPointContext.Database.BeginTransaction();
                try
                {
                    int userId = -1;

                    if (usermaster != null)
                    {
                        usermaster.Status = true;
                        usermaster.CreatedOn = DateTime.Now;
                        usermaster.IsFirstLogin = true;

                        _bugPointContext.UserMasters.Add(usermaster);
                        _bugPointContext.SaveChanges();
                        userId = usermaster.UserId;

                        if (roleId != null)
                        {
                            var savedAssignedRoles = new AssignedRolesModel()
                            {
                                RoleId = roleId.Value,
                                UserId = userId,
                                AssignedRoleId = 0,
                                Status = true,
                                CreateDate = DateTime.Now
                            };
                            _bugPointContext.AssignedRoles.Add(savedAssignedRoles);
                        }

                        _bugPointContext.SaveChanges();

                        dbContextTransaction.Commit();
                        return userId;
                    }

                    return userId;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string UpdateUser(UserMasterModel usermaster, AssignedRolesModel assignedRoles)
        {
            try
            {
                if (usermaster != null)
                {
                    usermaster.ModifiedOn = DateTime.Now;
                    _bugPointContext.Entry(usermaster).State = EntityState.Modified;
                    _bugPointContext.Entry(assignedRoles).State = EntityState.Modified;

                    _bugPointContext.SaveChanges();
                    return "success";
                }

                return "failed";
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteUser(int? userId)
        {
            try
            {
                UserMasterModel usermaster = _bugPointContext.UserMasters.Find(userId);
                if (usermaster != null) _bugPointContext.UserMasters.Remove(usermaster);
                _bugPointContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePasswordandHistory(int? userId, string passwordHash, string processType)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {

                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@PasswordHash", passwordHash);
                param.Add("@ProcessType", processType);
                var result = connection.Execute("Usp_PasswordMaster_UpdatePassword", param, transaction, 0, CommandType.StoredProcedure);

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

        public string ChangeUserStatus(UserMasterModel usermaster)
        {
            try
            {
                _bugPointContext.Entry(usermaster).State = EntityState.Modified;
                _bugPointContext.SaveChanges();
                return "success";
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string UpdatePassword(string password, int? userId)
        {
            using SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlConnectionManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@Password", password);
                var result = connection.Execute("CSC_USP_UpdatePassword", param, transaction, 0, CommandType.StoredProcedure);

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

        public bool UpdateIsFirstLoginStatus(int? userId)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {

                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                var result = connection.Execute("Usp_UpdateIsFirstLoginStatus", param, transaction, 0, CommandType.StoredProcedure);

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
    }
}