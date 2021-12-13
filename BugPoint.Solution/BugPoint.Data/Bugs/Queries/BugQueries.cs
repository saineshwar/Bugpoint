using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BugPoint.Data.EFContext;
using BugPoint.Model.Bugs;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.BugsList;
using BugPoint.ViewModel.MenuMaster;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace BugPoint.Data.Bugs.Queries
{
    public class BugQueries : IBugQueries
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        public BugQueries(BugPointContext bugPointContext, IConfiguration configuration)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
        }

        public BugSummaryModel GetBugSummarybybugId(long? bugId)
        {
            try
            {
                var editmenu = (from bugSummary in _bugPointContext.BugSummaryModel.AsNoTracking()
                                where bugSummary.BugId == bugId
                                select bugSummary).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BugDetailsModel GetBugsDetailsbybugId(long? bugId)
        {
            try
            {
                var editmenu = (from bugDetails in _bugPointContext.BugDetailsModel.AsNoTracking()
                                where bugDetails.BugId == bugId
                                select bugDetails).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BugTrackingModel GetBugTrackingbybugId(long? bugId)
        {
            try
            {
                var bug = (from bugTracking in _bugPointContext.BugTrackingModel.AsNoTracking()
                           where bugTracking.BugId == bugId
                           select bugTracking).FirstOrDefault();

                return bug;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AttachmentsModel> GetListAttachmentsBybugId(long? bugId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _bugPointContext.AttachmentsModel.AsNoTracking()
                                       where attachments.BugId == bugId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AttachmentsModel GetAttachmentsBybugId(long bugId, long attachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _bugPointContext.AttachmentsModel.AsNoTracking()
                                       where attachments.BugId == bugId && attachments.AttachmentId == attachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AttachmentDetailsModel GetAttachmentDetailsByAttachmentId(long bugId, long attachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _bugPointContext.AttachmentDetailsModel.AsNoTracking()
                                       where attachments.BugId == bugId && attachments.AttachmentId == attachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AttachmentsExistbybugId(long? bugId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _bugPointContext.AttachmentsModel
                                       where attachments.BugId == bugId
                                       select attachments.AttachmentId).Any();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetReportersBugsCount(int? createdBy, int? projectId, int? projectComponentId, int? priorityId, int? severityId, int? statusId, int? assignedtoId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@CreatedBy", createdBy);
            para.Add("@ProjectId", projectId);
            para.Add("@PriorityId", priorityId);
            para.Add("@SeverityId", severityId);
            para.Add("@StatusId", statusId);
            para.Add("@AssignedtoId", assignedtoId);
            para.Add("@ProjectComponentId", projectComponentId);
            var data = con.Query<int>("Usp_BugListGridCount", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return data;
        }

        public List<BugListGrid> GetReportersBugsList(int? createdBy, int? projectId, int? projectComponentId, int? priorityId,
            int? severityId, int? statusId, int? assignedtoId, int? pageNumber, int? pageSize)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@CreatedBy", createdBy);
            para.Add("@ProjectId", projectId);
            para.Add("@PriorityId", priorityId);
            para.Add("@SeverityId", severityId);
            para.Add("@StatusId", statusId);
            para.Add("@AssignedtoId", assignedtoId);
            para.Add("@ProjectComponentId", projectComponentId);
            para.Add("@page", pageNumber);
            para.Add("@pageSize", pageSize);
            var data = con.Query<BugListGrid>("Usp_BugListGrid", para, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }

        public BugDetailViewModel GetBugDetailsbyBugId(long? bugId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@BugId", bugId);
                var data = con.Query<BugDetailViewModel>("Usp_GetBugDetailsbyBugId", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ViewBugReplyHistoryModel> ListofHistoryTicketReplies(long? bugId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@BugId", bugId);
                return con.Query<ViewBugReplyHistoryModel>("Usp_Bug_HistorybugRepliesbyBugId", param, null, false, 0, CommandType.StoredProcedure).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ReplyAttachmentModel> GetListReplyAttachmentsByAttachmentId(long? bugId, long? bugReplyId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _bugPointContext.ReplyAttachmentModel
                                       where attachments.BugId == bugId && attachments.BugReplyId == bugReplyId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ReplyAttachmentModel GetReplyAttachmentsBybugId(long bugId, long replyAttachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _bugPointContext.ReplyAttachmentModel.AsNoTracking()
                                       where attachments.BugId == bugId && attachments.ReplyAttachmentId == replyAttachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ReplyAttachmentDetailsModel GetReplyAttachmentDetailsByAttachmentId(long bugId, long replyAttachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _bugPointContext.ReplyAttachmentDetailsModel.AsNoTracking()
                                       where attachments.BugId == bugId && attachments.ReplyAttachmentId == replyAttachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EditBugReplyViewModel GetBugReplyEditDetailsbyBugId(long? bugId, long? bugReplyId)
        {
            try
            {
                var data = (from bugReply in _bugPointContext.BugReplyDetailsModel.AsNoTracking()
                            where bugReply.BugId == bugId && bugReply.BugReplyId == bugReplyId
                            select new EditBugReplyViewModel()
                            {
                                BugId = bugReply.BugId,
                                BugReplyId = bugReply.BugReplyId,
                                Description = bugReply.Description,
                                BugReplyDetailsId = bugReply.BugReplyDetailsId
                            }).FirstOrDefault();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BugReplyDetailsModel GetBugReplyDetailsbyBugId(long? bugId, long? bugReplyId)
        {
            try
            {
                var data = (from bugReply in _bugPointContext.BugReplyDetailsModel.AsNoTracking()
                            where bugReply.BugId == bugId && bugReply.BugReplyId == bugReplyId
                            select bugReply).FirstOrDefault();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BugReplyModel GetBugReplybyBugId(long? bugId, long? bugReplyId)
        {
            try
            {
                var data = (from bugReply in _bugPointContext.BugReplyModel.AsNoTracking()
                            where bugReply.BugId == bugId && bugReply.BugReplyId == bugReplyId
                            select bugReply).FirstOrDefault();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ReplyAttachmentsExistbybugId(long? bugId, long? bugReplyId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _bugPointContext.ReplyAttachmentModel
                                       where attachments.BugId == bugId && attachments.BugReplyId == bugReplyId
                                       select attachments.ReplyAttachmentId).Any();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BugHistoryViewModel> GetBugHistorybyBugId(long? bugId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@BugId", bugId);
                var data = con.Query<BugHistoryViewModel>("Usp_GetBugHistorybyBugId", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public int GetMyBugsCount(int? currentuser, int? projectId, int? projectComponentId, int? priorityId, int? severityId, int? statusId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@AssignedTo", currentuser);
            para.Add("@ProjectId", projectId);
            para.Add("@PriorityId", priorityId);
            para.Add("@SeverityId", severityId);
            para.Add("@StatusId", statusId);
            para.Add("@ProjectComponentId", projectComponentId);
            var data = con.Query<int>("Usp_MyBugListGridCount", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return data;
        }

        public List<BugListGrid> GetMyBugsList(int? currentuser, int? projectId, int? projectComponentId, int? priorityId, int? severityId, int? statusId, int? pageNumber, int? pageSize)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@AssignedTo", currentuser);
            para.Add("@ProjectId", projectId);
            para.Add("@PriorityId", priorityId);
            para.Add("@SeverityId", severityId);
            para.Add("@StatusId", statusId);
            para.Add("@ProjectComponentId", projectComponentId);
            para.Add("@page", pageNumber);
            para.Add("@pageSize", pageSize);
            var data = con.Query<BugListGrid>("Usp_MyBugListGrid", para, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }

        public int GetMyBugsCountLastSevenDays(int? currentuser)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@AssignedTo", currentuser);
            var data = con.Query<int>("Usp_MyBugListGridCount_LastSevenDays", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return data;
        }
        public List<BugListGrid> GetMyBugsListLastSevenDays(int? currentuser, int? pageNumber, int? pageSize)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@AssignedTo", currentuser);
            para.Add("@page", pageNumber);
            para.Add("@pageSize", pageSize);
            var data = con.Query<BugListGrid>("Usp_MyBugListGrid_LastSevenDays", para, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }


    
        public int GetMyTeamsBugsCount(int? currentuser, int? projectId, int? projectComponentId, int? priorityId, int? severityId, int? statusId, int? devId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            para.Add("@DevUserId", devId);
            para.Add("@PriorityId", priorityId);
            para.Add("@SeverityId", severityId);
            para.Add("@StatusId", statusId);
            para.Add("@ProjectComponentId", projectComponentId);
            var data = con.Query<int>("Usp_MyTeamsBugListGridCount", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return data;
        }


        public int GetMyTeamsBugsCountLastSevenDays(int? currentuser)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@DevUserId", currentuser);
            var data = con.Query<int>("Usp_MyTeamsBugListGridCount_LastSevenDays", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return data;
        }

        public List<BugListGrid> GetMyTeamsBugsListLastSevenDays(int? currentuser,  int? pageNumber, int? pageSize)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@DevUserId", currentuser);
            para.Add("@page", pageNumber);
            para.Add("@pageSize", pageSize);
            var data = con.Query<BugListGrid>("Usp_MyTeamsBugListGrid_LastSevenDays", para, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }

        public List<BugListGrid> GetMyTeamsBugsList(int? currentuser, int? projectId, int? projectComponentId, int? priorityId, int? severityId, int? statusId,int? devId, int? pageNumber, int? pageSize)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            para.Add("@DevUserId", devId);
            para.Add("@PriorityId", priorityId);
            para.Add("@SeverityId", severityId);
            para.Add("@StatusId", statusId);
            para.Add("@ProjectComponentId", projectComponentId);
            para.Add("@page", pageNumber);
            para.Add("@pageSize", pageSize);
            var data = con.Query<BugListGrid>("Usp_MyTeamsBugListGrid", para, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }

        public int GetMyReportersBugsCount(int? createdBy, int? projectId, int? projectComponentId, int? priorityId, int? severityId, int? reportersUserId, int? statusId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            para.Add("@ReportersUserId", reportersUserId);
            para.Add("@PriorityId", priorityId);
            para.Add("@SeverityId", severityId);
            para.Add("@StatusId", statusId);
            para.Add("@ProjectComponentId", projectComponentId);
            var data = con.Query<int>("Usp_MyReportersBugListGridCount", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return data;
        }

        public List<BugListGrid> GetmyReportersBugsList(int? createdBy,
            int? projectId, int? projectComponentId, int? priorityId, 
            int? severityId, int? statusId,  int? reportersUserId, int? pageNumber, int? pageSize)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@ProjectId", projectId);
            para.Add("@ReportersUserId", reportersUserId);
            para.Add("@PriorityId", priorityId);
            para.Add("@SeverityId", severityId);
            para.Add("@StatusId", statusId);
            para.Add("@ProjectComponentId", projectComponentId);
            para.Add("@page", pageNumber);
            para.Add("@pageSize", pageSize);
            var data = con.Query<BugListGrid>("Usp_MyReportersBugListGrid", para, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }


        public int GetMyReportersBugsCountLastSevenDays(int? createdBy)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@UserId", createdBy);
            var data = con.Query<int>("Usp_MyReportersBugListGridCount_LastSevenDays", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return data;
        }

        public List<BugListGrid> GetmyReportersBugsListLastSevenDays(int? createdBy, int? pageNumber, int? pageSize)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@UserId", createdBy);
            para.Add("@page", pageNumber);
            para.Add("@pageSize", pageSize);
            var data = con.Query<BugListGrid>("Usp_MyReportersBugListGrid_LastSevenDays", para, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }



        public int GetManagerBugsCount(int? projectId,
            int? projectComponentId,
            int? priorityId,
            int? severityId,
            int? statusId,
            int? versionId,
            int? operatingSystemId,
            int? hardwareId,
            int? browserId,
            int? webFrameworkId,
            int? testedOnId,
            int? bugTypeId,
            int? reportersUserId,
            int? developersUserId,
            int? page)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@StatusId", statusId);
            para.Add("@ProjectId", projectId);
            para.Add("@ProjectComponentId", projectComponentId);
            para.Add("@SeverityId", severityId);
            para.Add("@PriorityId", priorityId);
            para.Add("@VersionId", versionId);
            para.Add("@OperatingSystemId", operatingSystemId);
            para.Add("@HardwareId", hardwareId);
            para.Add("@BrowserId", browserId);
            para.Add("@WebFrameworkId", webFrameworkId);
            para.Add("@TestedOnId", testedOnId);
            para.Add("@BugTypeId", bugTypeId);
            para.Add("@ReportersUserId", reportersUserId);
            para.Add("@DevelopersUserId", developersUserId);

            var data = con.Query<int>("Usp_MyManagerBugCount", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return data;
        }
        
        public List<BugListGrid> GetManagerBugsList(int? projectId,
            int? projectComponentId,
            int? priorityId,
            int? severityId,
            int? statusId,
            int? versionId,
            int? operatingSystemId,
            int? hardwareId,
            int? browserId,
            int? webFrameworkId,
            int? testedOnId,
            int? bugTypeId,
            int? reportersUserId,
            int? developersUserId,
            int? pageNumber, int? pageSize)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@StatusId", statusId);
            para.Add("@ProjectId", projectId);
            para.Add("@ProjectComponentId", projectComponentId);
            para.Add("@SeverityId", severityId);
            para.Add("@PriorityId", priorityId);
            para.Add("@VersionId", versionId);
            para.Add("@OperatingSystemId", operatingSystemId);
            para.Add("@HardwareId", hardwareId);
            para.Add("@BrowserId", browserId);
            para.Add("@WebFrameworkId", webFrameworkId);
            para.Add("@TestedOnId", testedOnId);
            para.Add("@BugTypeId", bugTypeId);
            para.Add("@ReportersUserId", reportersUserId);
            para.Add("@DevelopersUserId", developersUserId);
            para.Add("@page", pageNumber);
            para.Add("@pageSize", pageSize);
            var data = con.Query<BugListGrid>("Usp_MyManagerBugListGrid", para, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }


        public BugRecipientDetailsViewModel GetBugAssigntobybugId(long? bugId)
        {
            try
            {
                var bug = (from bugTracking in _bugPointContext.BugTrackingModel.AsNoTracking()
                    join userMaster in _bugPointContext.UserMasters on bugTracking.AssignedTo equals userMaster.UserId 
                    where bugTracking.BugId == bugId
                    select new BugRecipientDetailsViewModel()
                    {
                        RecipientFullName = $"{userMaster.FirstName} {userMaster.LastName}",
                        RecipientEmailId = userMaster.EmailId
                    }).FirstOrDefault();

                return bug;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BugRecipientDetailsViewModel GetBugTesterbybugId(long? bugId)
        {
            try
            {
                var bug = (from bugTracking in _bugPointContext.BugTrackingModel.AsNoTracking()
                    join userMaster in _bugPointContext.UserMasters on bugTracking.CreatedBy equals userMaster.UserId
                    where bugTracking.BugId == bugId
                    select new BugRecipientDetailsViewModel()
                    {
                        RecipientFullName = $"{userMaster.FirstName} {userMaster.LastName}",
                        RecipientEmailId = userMaster.EmailId
                    }).FirstOrDefault();

                return bug;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public int GetReportersBugsCountLastSevenDays(int? createdBy)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@CreatedBy", createdBy);
           
            var data = con.Query<int>("Usp_BugListGridCount_LastSevenDays", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return data;
        }

        public List<BugListGrid> GetReportersBugsListLastSevenDays(int? createdBy, int? pageNumber, int? pageSize)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@CreatedBy", createdBy);
            para.Add("@page", pageNumber);
            para.Add("@pageSize", pageSize);
            var data = con.Query<BugListGrid>("Usp_BugListGrid_LastSevenDays", para, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }

    }
}