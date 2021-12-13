using System.Collections.Generic;
using System.Linq;
using BugPoint.Model.Project;
using BugPoint.ViewModel.Project;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.Data.Project.Queries
{
    public interface IProjectQueries
    {
        bool CheckProjectNameExists(string projectname);
        IQueryable<ProjectGrid> ShowAllProjects(string sortColumn, string sortColumnDir, string search);
        EditProjectViewModel EditProject(int? projectId);
        ProjectsModel GetProjectDetails(int? projectId);
        List<SelectListItem> GetProjectbyProjectName(string projectname);
        List<SelectListItem> GetProjectList();

        List<SelectListItem> GetAllProjectList();
        List<SelectListItem> GetAssignedProjectList(int? userId);

        List<SelectListItem> GetAssignedProjectListWithoutSelect(int? userId);
    }
}