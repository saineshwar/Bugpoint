using BugPoint.Model.Assigned;

namespace BugPoint.Data.Assigned.Command
{
    public interface IAssignProjectCommand
    {
        int Add(AssignedProjectModel assignedProject);
        int Update(AssignedProjectModel assignedProject);
        int Delete(AssignedProjectModel assignedProject);
    }
}