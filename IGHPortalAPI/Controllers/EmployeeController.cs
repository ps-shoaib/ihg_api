using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.EmployeeViewModels;
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
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly IEmployeeService employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            this.logger = logger;
            this.employeeService = employeeService;
        }


        [HttpGet]
        public IActionResult GetAll(int hotelId)
        {
            var Employees = employeeService.GetEmployees(hotelId);
            return Ok(Employees);
        }


        [HttpGet("GetActiveDepartmentEmployees")]
        public IActionResult GetActiveDepartmentEmployees(int hotelId, int PayrollReportId = 0)
        {
            var Employees = employeeService.GetActiveDepartmentEmployees(hotelId, PayrollReportId);
            return Ok(Employees);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var Employee = employeeService.GetEmployee(id);
            return Ok(Employee);
        }

        



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeViewModel model)
        {
            if (employeeService.IsEmailAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Employee with given Email already exist try with different email");

                return BadRequest(response);
            }

            await employeeService.AddEmployee(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EmployeeViewModel model)
        {

            if (employeeService.IsEmailAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Employee with given Email already exist try with different email");

                return BadRequest(response);
            }

            await employeeService.UpdateEmployee(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await employeeService.DeleteEmployee(id);
            return Ok();
        }
    }

}
