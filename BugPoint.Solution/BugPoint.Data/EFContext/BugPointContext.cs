using BugPoint.Model.ApplicationLog;
using BugPoint.Model.Assigned;
using BugPoint.Model.AssignedRoles;
using BugPoint.Model.Audit;
using BugPoint.Model.Bugs;
using BugPoint.Model.GeneralSetting;
using BugPoint.Model.Masters;
using BugPoint.Model.MenuCategory;
using BugPoint.Model.MenuMaster;
using BugPoint.Model.MovingBugs;
using BugPoint.Model.Project;
using BugPoint.Model.RoleMaster;
using BugPoint.Model.UserMaster;
using Microsoft.EntityFrameworkCore;

namespace BugPoint.Data.EFContext
{
    public class BugPointContext : DbContext
    {
        public BugPointContext(DbContextOptions<BugPointContext> options) : base(options)
        {

        }

        public DbSet<MenuCategoryModel> MenuCategorys { get; set; }
        public DbSet<RoleMasterModel> RoleMasters { get; set; }
        public DbSet<MenuMasterModel> MenuMasters { get; set; }
        public DbSet<UserMasterModel> UserMasters { get; set; }
        public DbSet<AssignedRolesModel> AssignedRoles { get; set; }
        public DbSet<AuditModel> AuditModel { get; set; }
        public DbSet<PriorityModel> PriorityModel { get; set; }
        public DbSet<StatusModel> StatusModel { get; set; }
        public DbSet<SeverityModel> SeverityModel { get; set; }
        public DbSet<ResolutionModel> ResolutionModel { get; set; }
        public DbSet<HardwareModel> HardwareModel { get; set; }
        public DbSet<OperatingSystemModel> OperatingSystemModels { get; set; }
        public DbSet<VersionModel> VersionModel { get; set; }
        public DbSet<BrowserModel> BrowserModel { get; set; }
        public DbSet<BugTypesModel> BugTypesModel { get; set; }
        public DbSet<ProjectsModel> ProjectsModel { get; set; }
        public DbSet<AssignedProjectModel> AssignedProjectModel { get; set; }
        public DbSet<ProjectComponentModel> ProjectComponentModel { get; set; }
        public DbSet<WebFrameworkModel> WebFrameworkModel { get; set; }
        public DbSet<TestedEnvironmentModel> TestedEnvironmentModel { get; set; }
        public DbSet<BugSummaryModel> BugSummaryModel { get; set; }
        public DbSet<BugDetailsModel> BugDetailsModel { get; set; }
        public DbSet<BugTrackingModel> BugTrackingModel { get; set; }
        public DbSet<AttachmentsModel> AttachmentsModel { get; set; }
        public DbSet<AttachmentDetailsModel> AttachmentDetailsModel { get; set; }
        public DbSet<BugReplyDetailsModel> BugReplyDetailsModel { get; set; }
        public DbSet<BugReplyModel> BugReplyModel { get; set; }
        public DbSet<ReplyAttachmentModel> ReplyAttachmentModel { get; set; }
        public DbSet<ReplyAttachmentDetailsModel> ReplyAttachmentDetailsModel { get; set; }
        public DbSet<BugHistoryModel> BugHistoryModel { get; set; }
        public DbSet<SmtpEmailSettingsModel> SmtpEmailSettingsModel { get; set; }
        public DbSet<NLogModel> NLogModel { get; set; }
        public DbSet<MovedBugsHistory> MovedBugsHistory { get; set; }
        public DbSet<DesignationModel> DesignationModel { get; set; }
    }
}