using BugPoint.Model.Bugs;

namespace BugPoint.Data.Bugs.Command
{
    public interface IBugHistoryCommand
    {
        bool InsertBugHistory(BugHistoryModel bugHistory);
    }
}