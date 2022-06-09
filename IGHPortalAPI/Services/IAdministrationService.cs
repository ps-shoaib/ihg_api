using IGHportalAPI.ViewModels.AccountViewModels;
using IGHportalAPI.ViewModels.AdministrationViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IGHportalAPI.Services
{
    public interface IAdministrationService
    {

        Task<IdentityResult> CreateRoleAsync(RoleViewModel roleViewModel);

        Task AddUserAccount(UserViewModel userViewModel);

        Task<IdentityResult> EditRolePostAsync(RoleViewModel DTO);

        bool UserAlreadyExist(string Email);

        Task<RoleViewModel> GetRoleByID(string Id);

        IEnumerable<RoleViewModel> GetAllRoles();

        Task<IdentityResult> DeleteRole(string Id);

        Task<IEnumerable<UserViewModel>> GetListUsers();

        Task<EditUserViewModel> EditUserGet(string Id);

        Task<List<AllRolesViewModel>> AllRolesInfo();

        Task<IdentityResult> EditUserPostAsync(EditUserViewModel userDTO);

        Task DeleteUser(string Id);


    }
}
