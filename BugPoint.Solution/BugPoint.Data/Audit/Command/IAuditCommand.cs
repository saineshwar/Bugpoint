using BugPoint.Model.Audit;

namespace BugPoint.Data.Audit.Command
{
    public interface IAuditCommand
    {
        void InsertAuditData(AuditModel objaudittb);
    }
}