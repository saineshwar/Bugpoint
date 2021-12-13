using BugPoint.Model.Project;

namespace BugPoint.Data.Project.Command
{
    public interface IProjectCommand
    {
        int Add(ProjectsModel projectsModel);
        int Update(ProjectsModel projectsModel);
        int Delete(ProjectsModel projectsModel);
    }
}