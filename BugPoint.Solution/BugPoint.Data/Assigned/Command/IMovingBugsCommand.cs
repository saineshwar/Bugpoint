using BugPoint.ViewModel.MovingBugs;

namespace BugPoint.Data.Assigned.Command
{
    public interface IMovingBugsCommand
    {
        bool MovingBugsProcess(MovingBugsResponse movingBugs, int? createdby);
    }
}