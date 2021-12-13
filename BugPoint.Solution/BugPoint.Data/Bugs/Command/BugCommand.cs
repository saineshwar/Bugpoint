using BugPoint.Data.EFContext;
using BugPoint.Model.Bugs;
using BugPoint.ViewModel.Bugs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Microsoft.Extensions.Logging;

namespace BugPoint.Data.Bugs.Command
{
    public class BugCommand : IBugCommand
    {
        private readonly BugPointContext _bugPointContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BugCommand> _logger;

        public BugCommand(BugPointContext bugPointContext,
            IConfiguration configuration, ILogger<BugCommand> logger)
        {
            _bugPointContext = bugPointContext;
            _configuration = configuration;
            _logger = logger;
        }

        public bool AddBug(BugSummaryModel bugSummaryModel, 
            BugDetailsModel bugDetailsModel, 
            BugTrackingModel bugTrackingModel, 
            List<BugAttachmentsViewModel> listofAttachment)
        {
            using var transactionScope = new TransactionScope();

            try
            {
                _bugPointContext.BugSummaryModel.Add(bugSummaryModel);
                _bugPointContext.SaveChanges();

                bugDetailsModel.BugSummaryId = bugSummaryModel.BugSummaryId;
                _bugPointContext.BugDetailsModel.Add(bugDetailsModel);
                _bugPointContext.SaveChanges();

                _bugPointContext.BugTrackingModel.Add(bugTrackingModel);
                _bugPointContext.SaveChanges();

                foreach (var attach in listofAttachment)
                {
                    var attachmentsModel = new AttachmentsModel()
                    {
                        AttachmentId = 0,
                        OriginalAttachmentName = attach.OriginalAttachmentName,
                        GenerateAttachmentName = attach.GenerateAttachmentName,
                        AttachmentType = attach.AttachmentType,
                        BugId = attach.BugId,
                        CreatedBy = attach.CreatedBy,
                        CreatedOn = attach.CreatedOn,
                        BucketName = attach.BucketName,
                        DirectoryName = attach.DirectoryName
                    };

                    _bugPointContext.AttachmentsModel.Add(attachmentsModel);
                    _bugPointContext.SaveChanges();

                    var attachmentDetailsModel = new AttachmentDetailsModel()
                    {
                        AttachmentDetailsId = 0,
                        BugId = attach.BugId,
                        CreatedBy = attach.CreatedBy,
                        AttachmentId = attachmentsModel.AttachmentId,
                        AttachmentBase64 = attach.AttachmentBase64
                    };

                    _bugPointContext.AttachmentDetailsModel.Add(attachmentDetailsModel);
                    _bugPointContext.SaveChanges();
                }

                _bugPointContext.SaveChanges();
                transactionScope.Complete();
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "BugCommand:AddBug");
                return false;
            }
        }

        public bool UpdateBug(BugSummaryModel bugSummaryModel, BugDetailsModel bugDetailsModel, BugTrackingModel bugTrackingModel, List<BugAttachmentsViewModel> listofAttachment)
        {

            try
            {
                using var transactionScope = new TransactionScope();
                _bugPointContext.Entry(bugSummaryModel).State = EntityState.Modified;
                _bugPointContext.SaveChanges();

                _bugPointContext.Entry(bugDetailsModel).State = EntityState.Modified;
                _bugPointContext.SaveChanges();

                _bugPointContext.Entry(bugTrackingModel).State = EntityState.Modified;
                _bugPointContext.SaveChanges();

                foreach (var attach in listofAttachment)
                {
                    var attachmentsModel = new AttachmentsModel()
                    {
                        AttachmentId = 0,
                        GenerateAttachmentName = attach.GenerateAttachmentName,
                        OriginalAttachmentName = attach.OriginalAttachmentName,
                        AttachmentType = attach.AttachmentType,
                        BugId = attach.BugId,
                        CreatedBy = attach.CreatedBy,
                        CreatedOn = attach.CreatedOn,
                        BucketName = attach.BucketName,
                        DirectoryName = attach.DirectoryName
                    };

                    _bugPointContext.AttachmentsModel.Add(attachmentsModel);
                    _bugPointContext.SaveChanges();

                    var attachmentDetailsModel = new AttachmentDetailsModel()
                    {
                        AttachmentDetailsId = 0,
                        BugId = attach.BugId,
                        CreatedBy = attach.CreatedBy,
                        AttachmentId = attachmentsModel.AttachmentId,
                        AttachmentBase64 = attach.AttachmentBase64
                    };

                    _bugPointContext.AttachmentDetailsModel.Add(attachmentDetailsModel);
                    _bugPointContext.SaveChanges();
                }


                transactionScope.Complete();

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "BugCommand:UpdateBug");
                return false;
            }
        }

        public bool DeleteAttachmentByAttachmentId(AttachmentsModel attachmentsModel, AttachmentDetailsModel attachmentDetailsModel)
        {
            using var transactionScope = new TransactionScope();
            try
            {

                _bugPointContext.Entry(attachmentsModel).State = EntityState.Deleted;
                _bugPointContext.SaveChanges();

                _bugPointContext.Entry(attachmentDetailsModel).State = EntityState.Deleted;
                _bugPointContext.SaveChanges();

                transactionScope.Complete();

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "BugCommand:DeleteAttachmentByAttachmentId");
                return false;
            }
        }

        public bool AddReply(BugReplyModel bugReplyModel, BugReplyDetailsModel bugReplyDetailsModel, List<BugAttachmentsViewModel> listofAttachment, BugTrackingModel bugTrackingModel)
        {
            using var transactionScope = new TransactionScope();
            try
            {
                _bugPointContext.BugReplyModel.Add(bugReplyModel);
                _bugPointContext.SaveChanges();

                bugReplyDetailsModel.BugReplyId = bugReplyModel.BugReplyId;
                _bugPointContext.BugReplyDetailsModel.Add(bugReplyDetailsModel);
                _bugPointContext.SaveChanges();

                _bugPointContext.Entry(bugTrackingModel).State = EntityState.Modified;
                _bugPointContext.SaveChanges();

                foreach (var attach in listofAttachment)
                {
                    var attachmentsModel = new ReplyAttachmentModel()
                    {
                        BugReplyId = bugReplyModel.BugReplyId,
                        GenerateAttachmentName = attach.GenerateAttachmentName,
                        OriginalAttachmentName = attach.OriginalAttachmentName,
                        AttachmentType = attach.AttachmentType,
                        BugId = attach.BugId,
                        CreatedBy = attach.CreatedBy,
                        CreatedOn = attach.CreatedOn,
                        BucketName =attach.BucketName,
                        DirectoryName =attach.DirectoryName
                    };

                    _bugPointContext.ReplyAttachmentModel.Add(attachmentsModel);
                    _bugPointContext.SaveChanges();

                    var attachmentDetailsModel = new ReplyAttachmentDetailsModel()
                    {
                        ReplyAttachmentDetailsId = 0,
                        BugId = attach.BugId,
                        CreatedBy = attach.CreatedBy,
                        ReplyAttachmentId = attachmentsModel.ReplyAttachmentId,
                        AttachmentBase64 = attach.AttachmentBase64,
                    };

                    _bugPointContext.ReplyAttachmentDetailsModel.Add(attachmentDetailsModel);
                    _bugPointContext.SaveChanges();
                }

                _bugPointContext.SaveChanges();
                transactionScope.Complete();
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "BugCommand:AddReply");
                return false;
            }
        }

        public bool UpdateReply(BugReplyModel bugReplyModel, 
            BugReplyDetailsModel bugReplyDetailsModel, 
            List<BugAttachmentsViewModel> listofAttachment)
        {
            using var transactionScope = new TransactionScope();
            try
            {
                _bugPointContext.Entry(bugReplyModel).State = EntityState.Modified;
                _bugPointContext.SaveChanges();

                _bugPointContext.Entry(bugReplyDetailsModel).State = EntityState.Modified;
                _bugPointContext.SaveChanges();

                if (listofAttachment != null)
                {
                    foreach (var attach in listofAttachment)
                    {
                        var attachmentsModel = new ReplyAttachmentModel()
                        {
                            BugReplyId = bugReplyModel.BugReplyId,
                            GenerateAttachmentName = attach.GenerateAttachmentName,
                            OriginalAttachmentName = attach.OriginalAttachmentName,
                            AttachmentType = attach.AttachmentType,
                            BugId = attach.BugId,
                            CreatedBy = attach.CreatedBy,
                            CreatedOn = attach.CreatedOn,
                            BucketName = attach.BucketName,
                            DirectoryName = attach.DirectoryName
                        };

                        _bugPointContext.ReplyAttachmentModel.Add(attachmentsModel);
                        _bugPointContext.SaveChanges();

                        var attachmentDetailsModel = new ReplyAttachmentDetailsModel()
                        {
                            ReplyAttachmentDetailsId = 0,
                            BugId = attach.BugId,
                            CreatedBy = attach.CreatedBy,
                            ReplyAttachmentId = attachmentsModel.ReplyAttachmentId,
                            AttachmentBase64 = attach.AttachmentBase64,
                        };

                        _bugPointContext.ReplyAttachmentDetailsModel.Add(attachmentDetailsModel);
                        _bugPointContext.SaveChanges();
                    }
                }

                transactionScope.Complete();
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "BugCommand:UpdateReply");
                return false;
            }
        }

        public bool ChangeBugPriority(ChangePriorityRequestModel changePriorityRequestModel)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                using SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@PriorityId", changePriorityRequestModel.PriorityId);
                param.Add("@BugId", changePriorityRequestModel.BugId);
                var result = con.Execute("Usp_ChangeBugsPriority", param, transaction, 0,
                    CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BugCommand:ChangeBugPriority");
                throw;
            }
        }

        public bool ChangeBugAssignedUser(ChangeAssignedUserRequestModel changeAssignedUser)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    var param = new DynamicParameters();
                    param.Add("@AssignedTo", changeAssignedUser.UserId);
                    param.Add("@BugId", changeAssignedUser.BugId);
                    var result = con.Execute("Usp_ChangeBugsAssignedUser", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BugCommand:ChangeBugAssignedUser");
                throw;
            }
        }

        public bool ChangeBugsAssignedTester(ChangeAssignedUserRequestModel changeAssignedUser)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    var param = new DynamicParameters();
                    param.Add("@AssignedTo", changeAssignedUser.UserId);
                    param.Add("@BugId", changeAssignedUser.BugId);
                    var result = con.Execute("Usp_ChangeBugsAssignedTester", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BugCommand:ChangeBugsAssignedTester");
                throw;
            }
        }

        public bool UpdatebugStatus(long bugId, int statusId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    var param = new DynamicParameters();
                    param.Add("@StatusId", statusId);
                    param.Add("@BugId", bugId);
                    var result = con.Execute("Usp_UpdateBugsStatus", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BugCommand:UpdatebugStatus");
                throw;
            }
        }

        public bool DeleteReplyAttachmentByAttachmentId(ReplyAttachmentModel replyAttachment, ReplyAttachmentDetailsModel replyAttachmentDetails)
        {
            using var transactionScope = new TransactionScope();
            try
            {

                _bugPointContext.Entry(replyAttachment).State = EntityState.Deleted;
                _bugPointContext.SaveChanges();

                _bugPointContext.Entry(replyAttachmentDetails).State = EntityState.Deleted;
                _bugPointContext.SaveChanges();

                transactionScope.Complete();

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "BugCommand:DeleteReplyAttachmentByAttachmentId");
                return false;
            }
        }

    }

}
