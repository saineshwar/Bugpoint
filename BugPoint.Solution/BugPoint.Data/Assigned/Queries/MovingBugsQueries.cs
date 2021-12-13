using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using BugPoint.Common;
using BugPoint.Data.EFContext;
using BugPoint.ViewModel.Assigned;
using BugPoint.ViewModel.MovingBugs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.Assigned.Queries
{
    public class MovingBugsQueries : IMovingBugsQueries
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        public MovingBugsQueries(BugPointContext bugPointContext, IConfiguration configuration)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
        }


        public IQueryable<MovingBugsGrid> ShowAllAssignedBugs(string sortColumn, string sortColumnDir, string search, int? projectid, int userId, int roleId)
        {
            try
            {
                var queryables = (from bugTracking in _bugPointContext.BugTrackingModel.AsNoTracking()

                                  join bugSummary in _bugPointContext.BugSummaryModel on bugTracking.BugId equals bugSummary.BugId
                                  join projects in _bugPointContext.ProjectsModel on bugSummary.ProjectId equals projects.ProjectId
                                  join usermaster in _bugPointContext.UserMasters on bugTracking.AssignedTo equals usermaster.UserId
                                  join status in _bugPointContext.StatusModel on bugTracking.StatusId equals status.StatusId

                                  select new MovingBugsGrid()
                                  {
                                      BugId = bugTracking.BugId,
                                      ProjectId = bugSummary.ProjectId,
                                      ProjectName = projects.ProjectName,
                                      Status = status.StatusName,
                                      Summary = bugSummary.Summary,
                                      AssignedTo = bugTracking.AssignedTo,
                                      CreatedBy = bugTracking.CreatedBy
                                  }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryables = queryables.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryables = queryables.OrderByDescending(x => x.ProjectId);
                }

                if ((int)RolesHelper.Roles.Tester == roleId || (int)RolesHelper.Roles.TesterTeamLead == roleId)
                {
                    queryables = queryables.Where(bugTracking =>
                        bugTracking.CreatedBy == userId && bugTracking.ProjectId == projectid);
                }

                if ((int)RolesHelper.Roles.Developer == roleId || (int)RolesHelper.Roles.DeveloperTeamLead == roleId)
                {
                    queryables = queryables.Where(bugTracking =>
                        bugTracking.AssignedTo == userId && bugTracking.ProjectId == projectid);
                }

                if ((int)RolesHelper.Roles.Reporter == roleId)
                {
                    queryables = queryables.Where(bugTracking =>
                        bugTracking.AssignedTo == userId && bugTracking.ProjectId == projectid);
                }


                return queryables;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}