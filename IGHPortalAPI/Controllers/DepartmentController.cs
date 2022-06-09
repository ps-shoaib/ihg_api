using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.DepartmentViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> logger;
        private readonly IDepartmentService departmentService;

        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentService departmentService)
        {
            this.logger = logger;
            this.departmentService = departmentService;
        }


        [HttpGet]
        public IActionResult Get(int hotelId)
        {
            var Departments = departmentService.GetDepartments(hotelId);
            return Ok(Departments);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var Department = departmentService.GetDepartment(id);
            return Ok(Department);
        }

        



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DepartmentViewModel model)
        {
            
            if (departmentService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Department with given name already exist try with different name");

                return BadRequest(response);
            }

            await departmentService.AddDepartment(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] DepartmentViewModel model)
        {

            if (departmentService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Department with given name already exist try with different name");

                return BadRequest(response);
            }

            await departmentService.UpdateDepartment(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await departmentService.DeleteDepartment(id);
            return Ok();
        }
    }

}
