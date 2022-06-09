using IGHportalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gym_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize("Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> logger;
        private readonly IDashboardService employeeService;

        public DashboardController(ILogger<DashboardController> logger, IDashboardService employeeService)
        {
            this.logger = logger;
            this.employeeService = employeeService;
        }

        [AllowAnonymous]

        [HttpGet]
        public IActionResult Get(int hotelId)
        {
            var DashboardData = employeeService.GetDashboardData(hotelId);
            return Ok(DashboardData);
        }


    }

}
