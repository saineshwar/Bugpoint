using System.Collections.Generic;
using System.Linq;
using BugPoint.Model.Assigned;
using BugPoint.ViewModel.Assigned;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.Data.Assigned.Queries
{
    public interface IProjectComponentQueries
    {
        bool CheckProjectComponentExists(int projectId, string componentName);
        IQueryable<ProjectComponentGrid> ShowAllProjectComponents(string sortColumn, string sortColumnDir,
            string search, int projectid);

        ProjectComponentModel ProjectComponentDetailsByProjectId(long projectComponentId);
        EditProjectComponentViewModel GetProjectComponentDetailsByProjectId(int projectComponentId);
        List<SelectListItem> GetProjectComponentsListByUserId(int? projectId, int? userId);
        List<SelectListItem> GetProjectComponentsList(int? projectId);
    }
}