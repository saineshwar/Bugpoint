using System.Collections.Generic;
using System.Linq;
using BugPoint.Model.RoleMaster;
using BugPoint.ViewModel.RoleMaster;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugPoint.Data.RoleMaster.Queries
{
    public interface IRoleQueries
    {
        bool CheckRoleNameExists(string roleName);
        IQueryable<RoleMasterGrid> ShowAllRoleMaster(string sortColumn, string sortColumnDir, string search);
        RoleMasterModel GetRoleMasterByroleId(int? roleId);
        EditRoleMasterViewModel GetRoleMasterForEditByroleId(int? roleId);
        List<SelectListItem> ListofRoles();
        List<SelectListItem> GetAllActiveRoles();
        List<SelectListItem> ListofDevandTesterLeadsRoles();
    }
}