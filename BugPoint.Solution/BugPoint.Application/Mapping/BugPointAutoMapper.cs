using AutoMapper;
using BugPoint.Model.Bugs;
using BugPoint.Model.GeneralSetting;
using BugPoint.Model.MenuCategory;
using BugPoint.Model.MenuMaster;
using BugPoint.Model.UserMaster;
using BugPoint.ViewModel.Bugs;
using BugPoint.ViewModel.GeneralSettings;
using BugPoint.ViewModel.MenuCategory;
using BugPoint.ViewModel.MenuMaster;
using BugPoint.ViewModel.UserMaster;

namespace BugPoint.Application.Mapping
{
    public class BugPointAutoMapper : Profile
    {
        public BugPointAutoMapper()
        {
            CreateMap<CreateMenuCategoryViewModel, MenuCategoryModel>();
            CreateMap<CreateUserViewModel, UserMasterModel>();
            CreateMap<CreateMenuMasterViewModel, MenuMasterModel>();
            CreateMap<BugViewModel, BugSummaryModel>();
            CreateMap<BugSummaryModel , EditBugViewModel>();
            CreateMap<EditBugViewModel, BugSummaryModel>();
            CreateMap<SmtpEmailSettingsViewModel, SmtpEmailSettingsModel>();
        }
    }
}