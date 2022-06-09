using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGHportalAPI.Models;
using IGHportalAPI.ViewModels.AccountViewModels;
using IGHportalAPI.ViewModels.AdministrationViewModels;
using IGHportalAPI.ViewModels.HotelViewModels;
using IGHportalAPI.Models;
using IGHportalAPI.ViewModels.EmployeeViewModels;
using IGHportalAPI.ViewModels.DepartmentViewModels;
using IGHportalAPI.ViewModels.LinenInventoryViewModels;
using IGHportalAPI.ViewModels.LinenItemViewModels;
using IGHportalAPI.ViewModels.SundriesShopProductViewModels;
using IGHportalAPI.ViewModels.WeeklyWrapUpViewModels;
using IGHportalAPI.ViewModels.WeeklyWrapUp_BankDepositViewModels;
using IGHportalAPI.ViewModels.WeeklyWrapUp_OperationsViewModels;
using IGHportalAPI.ViewModels.Scores_IssuesViewModels;

namespace IGHportalAPI.Services.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            //.ForMember(obj => obj.Projects, ac => ac.Ignore());



            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();

            CreateMap<User, EditUserViewModel>().ReverseMap();


            CreateMap<Hotel, HotelViewModel>().ReverseMap()
                .ForMember(obj => obj.Users, ac => ac.Ignore())
                .ForMember(obj => obj.Employees, ac => ac.Ignore());
                //.ForMember(obj => obj., ac => ac.Ignore());

            CreateMap<Employee, EmployeeViewModel>().ReverseMap();

            CreateMap<Department, DepartmentViewModel>().ReverseMap();

            CreateMap<LinenInventory, LinenInventoryDetailsViewModel>().ReverseMap();

            CreateMap<List<LinenInventory>, List<LinenInventoryDetailsViewModel>>().ReverseMap();


            CreateMap<LinenItem, LinenItemViewModel>().ReverseMap();

            CreateMap<SundriesShopProduct, SundriesShopProductViewModel>().ReverseMap();

            CreateMap<WeeklyWrapUp, WeeklyWrapUpViewModel>().ReverseMap();

            CreateMap<WeeklyWrapUp_BankDeposit, WeeklyWrapUp_BankDepositViewModel>().ReverseMap().ForMember(a => a.WeeklyWrapUp, b => b.Ignore());

            CreateMap<WeeklyWrapUp_Operations, WeeklyWrapUp_OperationsViewModel>().ReverseMap();
            CreateMap<WeeklyWrapUp_OperationsDetails, WeeklyWrapUp_OperationsDetailsViewModel>().ReverseMap();
            CreateMap<Scores_Issues, Scores_IssuesViewModel>().ReverseMap();

        }
    }
}
