using BugPoint.Model.Assigned;

namespace BugPoint.Data.Assigned.Command
{
    public interface IProjectComponentCommand
    {
        int Add(ProjectComponentModel projectComponent);
        int Update(ProjectComponentModel projectComponent);
        bool Delete(int? projectComponentId);
    }
}