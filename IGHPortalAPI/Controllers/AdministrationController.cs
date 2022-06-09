using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.AccountViewModels;
using IGHportalAPI.ViewModels.AdministrationViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGHportalAPI.Error;

namespace IGHportalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IAdministrationService administrationService;

        public AdministrationController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }



        [HttpGet("AllRoles")]
        public IActionResult RolesList()
        {
            var result = administrationService.GetAllRoles();

            

            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }

        /*-----------------------------------------*/

        [HttpGet("GetRolesInfo")]
        public Task<List<AllRolesViewModel>> GetRolesInfo()
        {
            var result = administrationService.AllRolesInfo();

            return result;
        }

        /*-----------------------------------------*/

        [HttpGet("GetRoleByID/{id}")]
        public async Task<IActionResult> GetRoleByID(string id)
        {
            var result = await administrationService.GetRoleByID(id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        /*[Authorize(Roles = "Admin")]*/
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRoleAsync(RoleViewModel roleDTO)
        {
            var result = await administrationService.CreateRoleAsync(roleDTO);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }

        /*-----------------------------------------*/
        [HttpPut("EditRole")]
        public async Task<IActionResult> EditRolePutAsync(RoleViewModel model)
        {
            var result = await administrationService.EditRolePostAsync(model);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        /*-----------------------------------------*/

        [HttpDelete("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRoleAsync(string Id)
        {
            var result = await administrationService.DeleteRole(Id);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }

        /*-----------------------------------------*/

        [HttpGet("AllUsers")]
        public Task<IEnumerable<UserViewModel>> UsersList()
        {
            var result = administrationService.GetListUsers();


            return result;
        }

        /*-----------------------------------------*/

        [HttpGet("EditUserGet/{id}")]
        public async Task<IActionResult> EditUserGetAsync(string id)
        {
            var result = await administrationService.EditUserGet(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        /*-----------------------------------------*/

        /*-----------------------------------------*/
        [HttpPut("EditUserPut/{id}")]
        public async Task<IActionResult> EditUserPutAsync(string id, EditUserViewModel model)
        {
            model.Id = id;
            var result = await administrationService.EditUserPostAsync(model);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        /*-----------------------------------------*/

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {


             await administrationService.DeleteUser(Id);


            return Ok();
            //if (Result.Succeeded) {  }
            //else
            //{ return BadRequest(); }

        }


        [HttpPost]
        [Route("CreateUserAccount")]
        public async Task<IActionResult> CreateUserAccount(UserViewModel model)
        {
            if (administrationService.UserAlreadyExist(model.Email))
            {
                APIResponse response = new APIResponse(400, "User with specified email already exist please try with different Email");

                return BadRequest(response);
            }

             await administrationService.AddUserAccount(model);

            
            return Ok();
            
        }


    }
}
