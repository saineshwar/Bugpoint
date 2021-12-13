using BugPoint.Data.EFContext;
using BugPoint.Model.Assigned;
using BugPoint.Model.Project;
using Microsoft.EntityFrameworkCore;

namespace BugPoint.Data.Assigned.Command
{
    public class AssignProjectCommand : IAssignProjectCommand
    {
        private readonly BugPointContext _bugPointContext;

        public AssignProjectCommand(BugPointContext bugPointContext)
        {
            _bugPointContext = bugPointContext;
        }

        public int Add(AssignedProjectModel assignedProject)
        {
            _bugPointContext.AssignedProjectModel.Add(assignedProject);
            return _bugPointContext.SaveChanges();
        }

        public int Update(AssignedProjectModel assignedProject)
        {
            _bugPointContext.Entry(assignedProject).State = EntityState.Modified;
            return _bugPointContext.SaveChanges();
        }

        public int Delete(AssignedProjectModel assignedProject)
        {
            _bugPointContext.Entry(assignedProject).State = EntityState.Deleted;
            return _bugPointContext.SaveChanges();
        }
    }
}