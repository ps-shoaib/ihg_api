using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//----------minor change
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IGHportalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService service;

        public AccountController(IAccountService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel signUpDTO)
        {

            if (await service.UserAlreadyExist(signUpDTO))
            {

                APIResponse response = new APIResponse(400, "User with specified email already exist please try with different Email");

                return BadRequest(response);


            }

            var result = await service.SignUpAsync(signUpDTO);

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }


        [Authorize]
        [HttpGet]
        [Route("GetUserByToken")]
        public Task<UserViewModel> GetUserByToken()
        {
            //var Obj =   User.Claims.AsQueryable();

            var UserEmail = User.FindFirst(ClaimTypes.Name).Value;

            var UserObj = service.GetUserDetailsByEmail(UserEmail);

            return UserObj;
        }


        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] SignInViewModel signIn)
        {
            var result = await service.LoginAsync(signIn);

            if (result == null)
            {
                APIResponse response = new APIResponse(401, "Invalid Email or password");

                return Unauthorized(response);
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("ConfirmEmail")]
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailViewModel confirmEmailViewModel)
        {
            var result = await service.ConfirmEmail(confirmEmailViewModel);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Error in confirming Email.....");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Logout")]
        public async Task<IActionResult> LogOut()
        {
            await service.Logout();

            return Ok();            
        }


        [HttpPost("forgotPassword")]
        public IActionResult ForgotPassword([FromBody] SignInViewModel signIn)
        {
            var Result = service.ForgotPassowrdAsync(signIn.Email);
    
            return Ok(Result);

        }


        [HttpPost("resetPassword")]
        public IActionResult ResetPassword([FromBody] SignUpViewModel signUpDTO)
        {
            var Result = service.ResetPassword(signUpDTO);

            return Ok(Result);
        }


        [HttpPost("resetEmail")]
        public IActionResult ResetEmail([FromBody] ChangeEmailViewModel DTO)
        {
            var Result = service.ResetEmail(DTO);

            return Ok(Result);
        }

    }
}
