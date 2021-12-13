using System.Collections.Generic;
using System.Linq;
using BugPoint.Model.UserMaster;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.UserMaster;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.Data.UserMaster.Queries
{
    public interface IUserMasterQueries
    {
        UserMasterModel GetUserById(long? userId);
        bool CheckUsernameExists(string username);
        UserMasterModel GetUserByUsername(string username);
        bool CheckEmailIdExists(string emailid);
        bool CheckMobileNoExists(string mobileno);
        List<SelectListItem> GetListofAdmin();
        CommonUserDetailsViewModel GetCommonUserDetailsbyUserName(string username);
        IQueryable<UserMasterGrid> ShowAllUsers(string sortColumn, string sortColumnDir, string search);

        IQueryable<UserMasterGrid> ShowAllUsersAdmin(string sortColumn, string sortColumnDir, string search);
        EditUserViewModel GetUserForEditByUserId(long? userId);
        UserMasterModel GetUserDetailsbyUserId(long? userId);
        List<SelectListItem> GetUserNameListbyUsername(string username, int roleId);
        List<SelectListItem> GetListofDevelopers(int projectId);
        List<RequestDevelopers> ListofDeveloper(ChangeAssignedDeveloperRequestModel changeAssigned);
        UserMasterModel GetUserdetailsbyEmailId(string emailid);
        List<SelectListItem> ListofUserSpecificUser(BugMovementRequestModel changeAssigned);
        UserProfileViewModel UserProfile(int? userId);
    }
}