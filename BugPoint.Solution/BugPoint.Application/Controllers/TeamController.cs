using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugPoint.Common;
using BugPoint.Data.Assigned.Queries;
using BugPoint.Data.Project.Queries;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.UserMaster;
using Microsoft.AspNetCore.Http;
using BugPoint.Application.Filters;

namespace BugPoint.Application.Controllers
{
    [SessionTimeOut]
    public class TeamController : Controller
    {
        private readonly IProjectQueries _projectQueries;
        private readonly IAssignProjectQueries _assignProjectQueries;

        public TeamController(IProjectQueries projectQueries, IAssignProjectQueries assignProjectQueries)
        {
            _projectQueries = projectQueries;
            _assignProjectQueries = assignProjectQueries;
        }

        public IActionResult Members()
        {
            var user = HttpContext.Session.GetInt32(AllSessionKeys.UserId);
            var listofprojects = _projectQueries.GetAssignedProjectListWithoutSelect(user);
            var teamMembersModel = new TeamMembersModel()
            {
                ListofProjects = listofprojects
            };
            return View(teamMembersModel);
        }

        public IActionResult GetTeamList(RequestProjectReporter requestBug)
        {
            var listofteam = _assignProjectQueries.GetProjectTeam(requestBug.ProjectId);
            return PartialView("_Team", listofteam);
        }

    }
}
