
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGHportalAPI.ViewModels.AccountViewModels;

namespace IGHportalAPI.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> SignUpAsync(SignUpViewModel signUpModel);
        Task<AuthViewModel> LoginAsync(SignInViewModel signInModel);
        Task<UserViewModel> GetUserDetailsByEmail(string Email);
        Task Logout();

        Task<bool> UserAlreadyExist(SignUpViewModel model);

        Task ForgotPassowrdAsync(string Email);

        Task ResetPassword(SignUpViewModel dTO);

        Task ResetEmail(ChangeEmailViewModel dTO);


        Task<IdentityResult> ConfirmEmail(ConfirmEmailViewModel model);

    }
}
