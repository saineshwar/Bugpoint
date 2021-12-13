using BugPoint.Data.EFContext;
using BugPoint.Model.Project;
using BugPoint.Model.RoleMaster;
using Microsoft.EntityFrameworkCore;

namespace BugPoint.Data.Project.Command
{
    public class ProjectCommand : IProjectCommand
    {
        private readonly BugPointContext _bugPointContext;
        public ProjectCommand(BugPointContext bugPointContext)
        {
            _bugPointContext = bugPointContext;
        }

        public int Add(ProjectsModel projectsModel)
        {
            _bugPointContext.ProjectsModel.Add(projectsModel);
            return _bugPointContext.SaveChanges();
        }

        public int Update(ProjectsModel projectsModel)
        {
            _bugPointContext.Entry(projectsModel).State = EntityState.Modified;
            return _bugPointContext.SaveChanges();
        }

        public int Delete(ProjectsModel projectsModel)
        {
            _bugPointContext.Entry(projectsModel).State = EntityState.Deleted;
            return _bugPointContext.SaveChanges();
        }
    }
}