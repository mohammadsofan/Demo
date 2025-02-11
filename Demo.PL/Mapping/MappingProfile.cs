using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels.Admin;
using Demo.PL.Areas.Dashboard.ViewModels.Card;
using Demo.PL.Areas.Dashboard.ViewModels.Category;
using Demo.PL.Areas.Dashboard.ViewModels.Coupon;
using Demo.PL.Areas.Dashboard.ViewModels.Slide;
using Demo.PL.Areas.Dashboard.ViewModels.SubCategory;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<RegisterViewModel, ApplicationUser>();
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CreateCategoryViewModel>().ReverseMap();
            CreateMap<Category, EditCategoryViewModel>().ReverseMap();
            CreateMap<SubCategoryViewModel, SubCategory>().ReverseMap();
            CreateMap<CreateSubCategoryViewModel, SubCategory>().ReverseMap();
            CreateMap<EditSubCategoryViewModel, SubCategory>().ReverseMap();          
            CreateMap<Coupon, CouponViewModel>().ReverseMap();
            CreateMap<Slide, SlideViewModel>();
            CreateMap<CreateSlideViewModel, PreviewSlideViewModel>();
            CreateMap<CreateSlideViewModel, Slide>();
            CreateMap<Slide, EditSlideViewModel>().ReverseMap();
            CreateMap<EditSlideViewModel, PreviewSlideViewModel>();
            CreateMap<Slide, PreviewSlideViewModel>();
            CreateMap<Category, HomeCategoryViewModel>();
            CreateMap<CreateCardViewModel, Card>();
            CreateMap<Card, CardViewModel>();
            CreateMap<Card, EditCardViewModel>().ReverseMap();
            CreateMap<Card, HomeCardViewModel>();
            CreateMap<Card, PreviewCardViewModel>();
        }
    }
}
