using BugPoint.Data.EFContext;
using BugPoint.Model.Audit;

namespace BugPoint.Data.Audit.Command
{
    public class AuditCommand : IAuditCommand
    {
        private readonly BugPointContext _bugPointContext;
        public AuditCommand(BugPointContext bugPointContext)
        {
            _bugPointContext = bugPointContext;
        }

        public void InsertAuditData(AuditModel objaudittb)
        {
            try
            {
                _bugPointContext.AuditModel.Add(objaudittb);
                _bugPointContext.SaveChanges();

            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}