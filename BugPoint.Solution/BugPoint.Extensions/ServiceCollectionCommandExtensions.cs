using BugPoint.Data.Assigned.Command;
using BugPoint.Data.Audit.Command;
using BugPoint.Data.Bugs.Command;
using BugPoint.Data.Masters.Command;
using BugPoint.Data.MenuCategory.Command;
using BugPoint.Data.MenuMaster.Command;
using BugPoint.Data.MenuOrdering.Command;
using BugPoint.Data.Project.Command;
using BugPoint.Data.RoleMaster.Command;
using BugPoint.Data.UserMaster.Command;
using BugPoint.Services.MailHelper;
using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BugPoint.Extensions
{
    public static class ServiceCollectionCommandExtensions
    {
        public static IServiceCollection AddServicesCommand(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IAuditCommand, AuditCommand>();
            services.AddTransient<IUserMasterCommand, UserMasterCommand>();
            services.AddTransient<IRoleCommand, RoleCommand>();
            services.AddTransient<IOrderingCommand, OrderingCommand>();
            services.AddTransient<IMenuMasterCommand, MenuMasterCommand>();
            services.AddTransient<IMenuCategoryCommand, MenuCategoryCommand>();
            services.AddTransient<IProjectCommand, ProjectCommand>();
            services.AddTransient<IAssignProjectCommand, AssignProjectCommand>();
            services.AddTransient<IProjectComponentCommand, ProjectComponentCommand>();
            services.AddTransient<IBugCommand, BugCommand>();
            services.AddTransient<IBugHistoryCommand, BugHistoryCommand>();
            services.AddTransient<ISmtpSettingsCommand, SmtpSettingsCommand>();
            services.AddTransient<IMailingService, MailingService>();
            services.AddTransient<IVerificationCommand, VerificationCommand>();
            services.AddTransient<IMovingBugsCommand, MovingBugsCommand>();
            
            return services;
        }
    }
}
