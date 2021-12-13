using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using BugPoint.Data.EFContext;
using BugPoint.Model.Assigned;
using BugPoint.ViewModel.Assigned;
using BugPoint.ViewModel.BugsList;
using BugPoint.ViewModel.UserMaster;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.Assigned.Queries
{
    public class AssignProjectQueries : IAssignProjectQueries
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        public AssignProjectQueries(BugPointContext bugPointContext, IConfiguration configuration)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
        }

        public bool CheckProjectAlreadyAssigned(int? projectId, int? userId)
        {
            var result = (from projectsModel in _bugPointContext.AssignedProjectModel.AsNoTracking()
                          where projectsModel.ProjectId == projectId && projectsModel.UserId == userId
                          select projectsModel).Any();

            return result;

        }

        public IQueryable<AssignProjectGrid> ShowAllAssignedProjects(string sortColumn, string sortColumnDir, string search, int projectid)
        {
            try
            {
                var queryables = (from assignedProject in _bugPointContext.AssignedProjectModel.AsNoTracking()
                                  where assignedProject.ProjectId == projectid
                                  join projects in _bugPointContext.ProjectsModel on assignedProject.ProjectId equals projects.ProjectId
                                  join roleMaster in _bugPointContext.RoleMasters on assignedProject.RoleId equals roleMaster.RoleId
                                  join usermaster in _bugPointContext.UserMasters on assignedProject.UserId equals usermaster.UserId
                                  join designation in _bugPointContext.DesignationModel on usermaster.DesignationId equals designation.DesignationId
                                  select new AssignProjectGrid()
                                  {
                                      AssignedProjectId = assignedProject.AssignedProjectId,
                                      ProjectName = projects.ProjectName,
                                      Status = assignedProject.Status == true ? "Active" : "InActive",
                                      CreatedOn = assignedProject.CreatedOn,
                                      RoleName = roleMaster.RoleName,
                                      UserName = usermaster.FirstName + ' ' + usermaster.LastName,
                                      Designation = designation.Designation,
                                      RoleId = assignedProject.RoleId
                                  }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryables = queryables.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryables = queryables.OrderByDescending(x => x.AssignedProjectId);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryables = queryables.Where(m => m.UserName.Contains(search) || m.UserName.Contains(search));
                }

                return queryables;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public AssignedProjectModel GetAssignedProject(int? assignedProjectId)
        {
            var result = (from projectsModel in _bugPointContext.AssignedProjectModel.AsNoTracking()
                          where projectsModel.AssignedProjectId == assignedProjectId
                          select projectsModel).FirstOrDefault();

            return result;

        }

        public List<SelectListItem> GetTestersandDevelopersAssignedtoProject(int? projectId, int? roleId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            para.Add("@RoleId", roleId);
            var data = con.Query<SelectListItem>("Usp_GetTestersandDevelopersAssignedtoProject", para, commandType: CommandType.StoredProcedure).ToList();

            data.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return data;
        }

        public List<SelectListItem> GetDevelopersandTeamLeadAssignedtoProject(int? projectId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            var data = con.Query<SelectListItem>("Usp_GetDevelopersandTeamLeadAssignedtoProject", para, commandType: CommandType.StoredProcedure).ToList();

            data.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return data;
        }

        public List<SelectListItem> GetDeveloperTeamLeadAssignedtoProject(int? projectId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            var data = con.Query<SelectListItem>("Usp_GetDeveloperTeamLeadAssignedtoProject", para, commandType: CommandType.StoredProcedure).ToList();

            data.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return data;
        }

        public List<SelectListItem> GetTestersandTeamLeadAssignedtoProject(int? projectId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            var data = con.Query<SelectListItem>("Usp_GetTestersandTeamLeadAssignedtoProject", para, commandType: CommandType.StoredProcedure).ToList();

            data.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return data;
        }


        public List<RequestDevelopers> GetDevelopersandTeamLeadAssignedtoProjectNotSelf(int? projectId, int? currentUserId, string username)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            para.Add("@CurrentUserId", currentUserId);
            var listofdev = con.Query<RequestDevelopers>("Usp_GetDevelopersandTeamLeadAssignedtoProjectbutSelf", para, commandType: CommandType.StoredProcedure).ToList();

            var returnlist = (from a in listofdev
                              where a.Username.ToLower().Contains(username)
                              select a).ToList();

            return returnlist;
        }

        public List<TeamMembers> GetProjectTeam(int? projectId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            var listofteam = con.Query<TeamMembers>("Usp_TeamDetailsbyProjectId", para, commandType: CommandType.StoredProcedure).ToList();
            return listofteam;
        }

        public bool CheckIsUserAssignedAlreadyinUse(int? assignedProjectId, int? roleId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@AssignedProjectId", assignedProjectId);
            para.Add("@RoleId", roleId);
            var data = con.Query<int>("Usp_CheckIsUserAssignedAlreadyinUse", para, commandType: CommandType.StoredProcedure).FirstOrDefault();

            switch (data > 0)
            {
                case true:
                    return true;
                default:
                    return false;
            }
        }
    }
}