using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<RegisterViewModel, ApplicationUser>();
            CreateMap<IdentityRole, RoleViewModel>();
            CreateMap<RoleViewModel, IdentityRole>();
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
            CreateMap<SubCategoryViewModel, SubCategory>();
        }
    }
}
