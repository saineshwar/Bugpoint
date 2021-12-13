using System.Collections.Generic;
using System.Linq;
using BugPoint.Model.Assigned;
using BugPoint.ViewModel.Assigned;
using BugPoint.ViewModel.UserMaster;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.Data.Assigned.Queries
{
    public interface IAssignProjectQueries
    {
        bool CheckProjectAlreadyAssigned(int? projectId, int? userId);
        IQueryable<AssignProjectGrid> ShowAllAssignedProjects(string sortColumn, string sortColumnDir, string search, int projectid);
        AssignedProjectModel GetAssignedProject(int? assignedProjectId);
        List<SelectListItem> GetTestersandDevelopersAssignedtoProject(int? projectId, int? roleId);
        List<SelectListItem> GetDevelopersandTeamLeadAssignedtoProject(int? projectId);
        List<SelectListItem> GetDeveloperTeamLeadAssignedtoProject(int? projectId);
        List<SelectListItem> GetTestersandTeamLeadAssignedtoProject(int? projectId);
        List<RequestDevelopers> GetDevelopersandTeamLeadAssignedtoProjectNotSelf(int? projectId, int? currentUserId,
            string username);

        List<TeamMembers> GetProjectTeam(int? projectId);
        bool CheckIsUserAssignedAlreadyinUse(int? assignedProjectId, int? roleId);
    }
}