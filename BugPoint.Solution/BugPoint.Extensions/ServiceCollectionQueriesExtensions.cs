

using BugPoint.Data.ApplicationLog.Queries;
using BugPoint.Data.Assigned.Queries;
using BugPoint.Data.Audit.Queries;
using BugPoint.Data.Bugs.Queries;
using BugPoint.Data.Charts.Queries;
using BugPoint.Data.Masters.Queries;
using BugPoint.Data.MenuCategory.Queries;
using BugPoint.Data.MenuMaster.Queries;
using BugPoint.Data.Project.Queries;
using BugPoint.Data.Reports.Queries;
using BugPoint.Data.RoleMaster.Queries;
using BugPoint.Data.UserMaster.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BugPoint.Extensions
{
    public static class ServiceCollectionQueriesExtensions
    {
        public static IServiceCollection AddServicesQueries(this IServiceCollection services,
            IConfiguration configuration)
        {
         
            services.AddTransient<IUserMasterQueries, UserMasterQueries>();
            services.AddTransient<IMenuCategoryQueries, MenuCategoryQueries>();
            services.AddTransient<IMenuMasterQueries, MenuMasterQueries>();
            services.AddTransient<IRoleQueries, RoleQueries>();
            services.AddTransient<IAssignedRolesQueries, AssignedRolesQueries>();
            services.AddTransient<IProjectQueries, ProjectQueries>();
            services.AddTransient<IAssignProjectQueries, AssignProjectQueries>();
            services.AddTransient<IProjectComponentQueries, ProjectComponentQueries>();
            services.AddTransient<IBugNumberGeneratorQueries, BugNumberGeneratorQueries>();
            services.AddTransient<IMastersQueries, MastersQueries>();
            services.AddTransient<IBugQueries, BugQueries>();
            services.AddTransient<IBugHistoryHelper, BugHistoryHelper>();
            services.AddTransient<IChartsQueries, ChartsQueries>();
            services.AddTransient<ISmtpSettingsQueries, SmtpSettingsQueries>();
            services.AddTransient<IVerificationQueries, VerificationQueries>();
            services.AddTransient<IReportQueries, ReportQueries>();
            services.AddTransient<IApplicationLogQueries, ApplicationLogQueries>();
            services.AddTransient<IMovingBugsQueries, MovingBugsQueries>();
            services.AddTransient<IAuditQueries, AuditQueries>();
            
            return services;
        }
    }
}