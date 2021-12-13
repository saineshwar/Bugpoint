using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BugPoint.Data.EFContext;
using BugPoint.Model.UserMaster;
using BugPoint.ViewModel.UserMaster;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Dynamic.Core;
using BugPoint.ViewModel.Bugs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.UserMaster.Queries
{
    public class UserMasterQueries : IUserMasterQueries
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        public UserMasterQueries(BugPointContext bugPointContext, IConfiguration configuration)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
        }

        public UserMasterModel GetUserById(long? userId)
        {
            try
            {
                var result = (from user in _bugPointContext.UserMasters.AsNoTracking()
                              where user.UserId == userId
                              select user).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckUsernameExists(string username)
        {
            try
            {
                var result = (from menu in _bugPointContext.UserMasters
                              where menu.UserName == username
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserMasterModel GetUserByUsername(string username)
        {
            try
            {
                var result = (from usermaster in _bugPointContext.UserMasters
                              where usermaster.UserName == username
                              select usermaster).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserMasterModel GetUserdetailsbyEmailId(string emailid)
        {
            try
            {
                var result = (from user in _bugPointContext.UserMasters
                              where user.EmailId == emailid
                              select user).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool CheckEmailIdExists(string emailid)
        {
            try
            {
                var result = (from menu in _bugPointContext.UserMasters
                              where menu.EmailId == emailid
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckMobileNoExists(string mobileno)
        {
            try
            {
                var result = (from menu in _bugPointContext.UserMasters
                              where menu.MobileNo == mobileno
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetListofAdmin()
        {
            try
            {

                var adminlist = (from usermaster in _bugPointContext.UserMasters
                                 join savedroles in _bugPointContext.AssignedRoles on usermaster.UserId equals savedroles.UserId
                                 where usermaster.Status == true && savedroles.RoleId == 3
                                 select new SelectListItem()
                                 {
                                     Text = usermaster.FirstName + " " + usermaster.LastName,
                                     Value = usermaster.UserId.ToString()
                                 }).ToList();

                adminlist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CommonUserDetailsViewModel GetCommonUserDetailsbyUserName(string username)
        {
            var userdata = (from tempuser in _bugPointContext.UserMasters
                            join assignedRoles in _bugPointContext.AssignedRoles on tempuser.UserId equals assignedRoles.UserId
                            join roleMaster in _bugPointContext.RoleMasters on assignedRoles.RoleId equals roleMaster.RoleId
                            where tempuser.UserName == username
                            select new CommonUserDetailsViewModel()
                            {
                                FirstName = tempuser.FirstName,
                                EmailId = tempuser.EmailId,
                                LastName = tempuser.LastName,
                                RoleId = roleMaster.RoleId,
                                UserId = tempuser.UserId,
                                RoleName = roleMaster.RoleName,
                                Status = tempuser.Status,
                                UserName = tempuser.UserName,
                                PasswordHash = tempuser.PasswordHash,
                                MobileNo = tempuser.MobileNo,
                                IsFirstLogin = tempuser.IsFirstLogin,
                                Gender = tempuser.Gender
                            }).FirstOrDefault();

            return userdata;
        }



        public IQueryable<UserMasterGrid> ShowAllUsers(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesUserMasters = (from userMaster in _bugPointContext.UserMasters
                                             join assignedRole in _bugPointContext.AssignedRoles on userMaster.UserId equals assignedRole.UserId
                                             join roles in _bugPointContext.RoleMasters on assignedRole.RoleId equals roles.RoleId
                                             join designation in _bugPointContext.DesignationModel on userMaster.DesignationId equals designation.DesignationId
                                             select new UserMasterGrid()
                                             {
                                                 CreatedOn = userMaster.CreatedOn,
                                                 EmailId = userMaster.EmailId,
                                                 FirstName = string.IsNullOrEmpty(userMaster.FirstName) ? "-" : userMaster.FirstName,
                                                 Gender = string.IsNullOrEmpty(userMaster.Gender) ? "-" : userMaster.Gender == "M" ? "Male" : "Female",
                                                 LastName = string.IsNullOrEmpty(userMaster.LastName) ? "-" : userMaster.LastName,
                                                 MobileNo = userMaster.MobileNo,
                                                 RoleName = roles.RoleName,
                                                 UserId = userMaster.UserId,
                                                 UserName = userMaster.UserName,
                                                 Status = userMaster.Status == true ? "Active" : "InActive",
                                                 RoleId = roles.RoleId,
                                                 IsFirstLogin = userMaster.IsFirstLogin == true ? "Yes":"No",
                                                 Designation = designation.Designation
                                             }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesUserMasters = queryablesUserMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryablesUserMasters = queryablesUserMasters.OrderByDescending(x => x.UserId);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    queryablesUserMasters = queryablesUserMasters.Where(m => m.UserName.Contains(search) || m.UserName.Contains(search));
                }

                return queryablesUserMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<UserMasterGrid> ShowAllUsersAdmin(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesUserMasters = (from userMaster in _bugPointContext.UserMasters
                                             join assignedRole in _bugPointContext.AssignedRoles on userMaster.UserId equals assignedRole.UserId
                                             join roles in _bugPointContext.RoleMasters on assignedRole.RoleId equals roles.RoleId
                                             join designation in _bugPointContext.DesignationModel on userMaster.DesignationId equals designation.DesignationId
                                             where roles.RoleId != 1 && roles.RoleId != 2
                                             select new UserMasterGrid()
                                             {
                                                 CreatedOn = userMaster.CreatedOn,
                                                 EmailId = userMaster.EmailId,
                                                 FirstName = string.IsNullOrEmpty(userMaster.FirstName) ? "-" : userMaster.FirstName,
                                                 Gender = string.IsNullOrEmpty(userMaster.Gender) ? "-" : userMaster.Gender == "M" ? "Male" : "Female",
                                                 LastName = string.IsNullOrEmpty(userMaster.LastName) ? "-" : userMaster.LastName,
                                                 MobileNo = userMaster.MobileNo,
                                                 RoleName = roles.RoleName,
                                                 UserId = userMaster.UserId,
                                                 UserName = userMaster.UserName,
                                                 Status = userMaster.Status == true ? "Active" : "InActive",
                                                 RoleId = roles.RoleId,
                                                 IsFirstLogin = userMaster.IsFirstLogin == true ? "Yes" : "No",
                                                 Designation = designation.Designation
                                             }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesUserMasters = queryablesUserMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryablesUserMasters = queryablesUserMasters.OrderByDescending(x => x.UserId);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    queryablesUserMasters = queryablesUserMasters.Where(m => m.UserName.Contains(search) || m.UserName.Contains(search));
                }

                return queryablesUserMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public EditUserViewModel GetUserForEditByUserId(long? userId)
        {
            var role = (from tempuser in _bugPointContext.UserMasters
                        join assignedRole in _bugPointContext.AssignedRoles on tempuser.UserId equals assignedRole.UserId
                        join roles in _bugPointContext.RoleMasters on assignedRole.RoleId equals roles.RoleId
                        where tempuser.UserId == userId
                        select new EditUserViewModel()
                        {
                            FirstName = tempuser.FirstName,
                            EmailId = tempuser.EmailId,
                            LastName = tempuser.LastName,
                            MobileNo = tempuser.MobileNo,
                            Gender = tempuser.Gender,
                            RoleId = roles.RoleId,
                            Status = roles.Status,
                            UserName = tempuser.UserName,
                            UserId = tempuser.UserId,
                            DesignationId = tempuser.DesignationId
                        }).FirstOrDefault();
            return role;
        }

        public UserMasterModel GetUserDetailsbyUserId(long? userId)
        {
            var userdata = (from tempuser in _bugPointContext.UserMasters
                            where tempuser.UserId == userId
                            select tempuser).FirstOrDefault();

            return userdata;
        }

        public List<SelectListItem> GetUserNameListbyUsername(string username, int roleId)
        {
            try
            {

                var adminlist = (from usermaster in _bugPointContext.UserMasters
                                 join savedroles in _bugPointContext.AssignedRoles on usermaster.UserId equals savedroles.UserId
                                 where usermaster.Status == true && usermaster.UserName.Contains(username) && savedroles.RoleId == roleId
                                 select new SelectListItem()
                                 {
                                     Text = usermaster.FirstName + " " + usermaster.LastName,
                                     Value = usermaster.UserId.ToString()
                                 }).ToList();


                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get All Developers
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetListofDevelopers(int projectId)
        {
            try
            {

                var datalist = (from assignedProject in _bugPointContext.AssignedProjectModel
                                join usermaster in _bugPointContext.UserMasters on assignedProject.UserId equals usermaster.UserId
                                where usermaster.Status == true && assignedProject.ProjectId == projectId && assignedProject.RoleId == 4
                                select new SelectListItem()
                                {
                                    Text = usermaster.FirstName + " " + usermaster.LastName,
                                    Value = usermaster.UserId.ToString()
                                }).ToList();

                datalist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });



                return datalist;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<RequestDevelopers> ListofDeveloper(ChangeAssignedDeveloperRequestModel changeAssigned)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@ProjectId", changeAssigned.ProjectId);
                var listofdev = con.Query<RequestDevelopers>("Usp_GetDeveloperListbyName", param, null, false, 0, CommandType.StoredProcedure).ToList();
                var returnlist = (from a in listofdev
                                  where a.Username.ToLower().Contains(changeAssigned.Username)
                                  select a).ToList();

                return returnlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

    

        public List<SelectListItem> ListofUserSpecificUser(BugMovementRequestModel changeAssigned)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@UserId", changeAssigned.UserId);
                param.Add("@Username", changeAssigned.Username);
                param.Add("@RoleId", changeAssigned.RoleId);

                var listofdev = con.Query<SelectListItem>("Usp_GetUserNamesWithoutSpecificUser", param, null, false, 0, CommandType.StoredProcedure).ToList();
               
                return listofdev;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserProfileViewModel UserProfile(int? userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                var userProfile = con.Query<UserProfileViewModel>("Usp_GetUserDetailforProfilebyUserId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                return userProfile;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}