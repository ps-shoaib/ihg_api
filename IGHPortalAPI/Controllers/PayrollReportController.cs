using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.PayrollReportViewModels;
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
    public class PayrollReportController : ControllerBase
    {
        private readonly ILogger<PayrollReportController> logger;
        private readonly IPayrollReportService payrollReportService;

        public PayrollReportController(ILogger<PayrollReportController> logger, IPayrollReportService payrollReportService)
        {
            this.logger = logger;
            this.payrollReportService = payrollReportService;
        }


        [HttpGet]
        public IActionResult GetAll(int hotelId , string Month = "", string Year = "")
        {
            var PayrollReports = payrollReportService.GetPayrollReports(hotelId, Month, Year);
            return Ok(PayrollReports);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id, int hotelId)
        {
            var PayrollReport = payrollReportService.GetPayrollReport(id, hotelId);
            return Ok(PayrollReport);
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> CopyReport(int id, [FromBody] PayrollReportViewModel model)
        {
            if (payrollReportService.IsGivenWeeksReportAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Given Weeks Report Already Exist");

                return BadRequest(response);
            }
            model.Id = id;
            await payrollReportService.CopyPayrollReport(model);
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PayrollReportViewModel model)
        {

            if (payrollReportService.IsGivenWeeksReportAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Given Weeks Report Already Exist");

                return BadRequest(response);
            }

            await payrollReportService.AddPayrollReport(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PayrollReportViewModel model)
        {
            if (payrollReportService.IsGivenWeeksReportAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Given Weeks Report Already Exist");

                return BadRequest(response);
            }

            await payrollReportService.UpdatePayrollReport(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await payrollReportService.DeletePayrollReport(id);
            return Ok();
        }
    }

}
