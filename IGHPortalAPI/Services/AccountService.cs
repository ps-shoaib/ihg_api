using AutoMapper;
using IGHportalAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using IGHportalAPI.DataContext;
using IGHportalAPI.Models;
using IGHportalAPI.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Http;

namespace IGHportalAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly DataContext_ db;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            DataContext_ db,
            IMapper mapper,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IEmailSender emailSender
           )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
            this.mapper = mapper;
            this.configuration = configuration;
            this.emailSender = emailSender;
        }




        public async Task<IdentityResult> SignUpAsync(SignUpViewModel signUpDTO)
        {


            var user = new User()
            {
                Email = signUpDTO.Email,
                UserName = signUpDTO.Email,
                FirstName = signUpDTO.FirstName,
                LastName = signUpDTO.LastName,
                IsActive = true
            };


            IdentityResult identityResult = await userManager.CreateAsync(user, signUpDTO.Password);

            if (!identityResult.Succeeded)
            {
                return null;
            }

            //SignInViewModel signInDTO = new SignInViewModel()
            //{
            //    Email = signUpDTO.Email,
            //    Password = signUpDTO.Password
            //};


            //var AuthResult = await LoginAsync(signInDTO);

            var UserFromDb = await userManager.FindByNameAsync(user.UserName);

            var Token = await userManager.GenerateEmailConfirmationTokenAsync(UserFromDb);



            var uriBuilder = new UriBuilder(configuration["ReturnPaths:ConfirmEmail"]);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["token"] = Token;
            query["userId"] = UserFromDb.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();


            await emailSender.SendEmailAsync(UserFromDb.Email, "Confirm Your Email Address", urlString);


            return identityResult;
        }

        public async Task<bool> UserAlreadyExist(SignUpViewModel model)
        {
            var User_ = await userManager.FindByEmailAsync(model.Email);
            if (User_ == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<UserViewModel> GetUserDetailsByEmail(string Email)
        {
                var UserObj = await userManager.FindByEmailAsync(Email);

                var User = mapper.Map<UserViewModel>(UserObj);

                return User;
        }

        public async Task ResetPassword(SignUpViewModel dTO)
        {
                var user = await userManager.FindByEmailAsync(dTO.Email);

                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                await userManager.ResetPasswordAsync(user, token, dTO.Password);
        }

        public async Task ResetEmail(ChangeEmailViewModel dTO)
        {
             var user = await userManager.FindByEmailAsync(dTO.OldEmail);

             var token = await userManager.GenerateChangeEmailTokenAsync(user, dTO.NewEmail);

             await userManager.ChangeEmailAsync(user, dTO.NewEmail, token);
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<AuthViewModel> LoginAsync(SignInViewModel signInDTO)
        {
            User user = await userManager.FindByEmailAsync(signInDTO.Email);

            if (user == null ||  user.IsActive != true)
            {
                return null;
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, signInDTO.Password, false);



            if (!result.Succeeded)
            {
                return null;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, signInDTO.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var RolesOfUser = await userManager.GetRolesAsync(user);

            foreach (var item in RolesOfUser)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            //configuration["JWT:Secret"]
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"]));
            
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
              );



            AuthViewModel auth = new AuthViewModel()
            {
                accessToken  = new JwtSecurityTokenHandler().WriteToken(token).ToString(),
                expiresIn    = DateTime.Now.AddDays(1),
                refreshToken = new JwtSecurityTokenHandler().WriteToken(token).ToString(),
                LoggedInUserId = user.Id,
                Roles = RolesOfUser,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Name = user.FirstName + " "+user.LastName,
                Id = user.Id
            };
            return auth;
        }
      
        public async Task ForgotPassowrdAsync(string Email)
        {

            User Userobj = await userManager.FindByEmailAsync(Email);


            string NewPass = Guid.NewGuid().ToString();


            var PassResetToken = await userManager.GeneratePasswordResetTokenAsync(Userobj);

            var ResetingResult = await userManager.ResetPasswordAsync(Userobj, PassResetToken, NewPass);

            if (ResetingResult.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<b>Congratulation</b>");
                sb.AppendLine("<p>You have successfully reseted your passowrd</p>");
                sb.AppendLine($"<br><br> <b>Your new Password Is :  {NewPass}</b>");
                sb.AppendLine("<br><p>You can reset this password later</p><br/>");
                sb.AppendLine("</p><p><b>Thank You!</b></p>");

                await emailSender.SendEmailAsync(Userobj.Email, "Reset Password", sb.ToString());

            }
        }

        public async Task<IdentityResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            var result = await userManager.ConfirmEmailAsync(user, model.Token);

            return result;

        }
    }
}
