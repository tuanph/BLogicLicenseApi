using AutoMapper;
using BLogicLicense.Model.Models;
using BLogicLicense.Service.ViewModels;
using BLogicLicense.Service.ViewModels.Common;
using BLogicLicense.Service.ViewModels.Software;
using BLogicLicense.Service.ViewModels.Store;
using BLogicLicense.Service.ViewModels.UnregisterKey;

namespace BLogicLicense.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Page, PageViewModel>();
                cfg.CreateMap<AppRole, ApplicationRoleViewModel>();
                cfg.CreateMap<AppUser, AppUserViewModel>();
                cfg.CreateMap<Function, FunctionViewModel>();
                cfg.CreateMap<Permission, PermissionViewModel>();
                cfg.CreateMap<Announcement, AnnouncementViewModel>();
                cfg.CreateMap<AnnouncementUser, AnnouncementUserViewModel>();
                cfg.CreateMap<Store, StoreViewModel>();
                cfg.CreateMap<Software, SoftwareViewModel>();
                cfg.CreateMap<UnRegisterKey, UnregisterKeyViewModel>();
            });
        }
    }
}