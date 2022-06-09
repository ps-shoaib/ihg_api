using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.WeeklyWrapUpViewModels;
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
    public class WeeklyWrapUpController : ControllerBase
    {
        private readonly ILogger<WeeklyWrapUpController> logger;
        private readonly IWeeklyWrapUpService WeeklyWrapUpService;

        public WeeklyWrapUpController(ILogger<WeeklyWrapUpController> logger, IWeeklyWrapUpService WeeklyWrapUpService)
        {
            this.logger = logger;
            this.WeeklyWrapUpService = WeeklyWrapUpService;
        }


        [HttpGet]
        public IActionResult Get(int hotelId)
        {
            var WeeklyWrapUps = WeeklyWrapUpService.GetWeeklyWrapUps(hotelId);
            return Ok(WeeklyWrapUps);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var WeeklyWrapUp = WeeklyWrapUpService.GetWeeklyWrapUp(id);
            return Ok(WeeklyWrapUp);
        }


        [HttpPost("PostFiles")]
        public async Task<IActionResult> PostFiles([FromForm] ICollection<IFormFile> filesArray)
        {

            var FilesURLs = await WeeklyWrapUpService.SaveFiles(filesArray);
            return Ok(FilesURLs);
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WeeklyWrapUpViewModel model)
        {

            if (WeeklyWrapUpService.IsWeeklyReportAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "WeeklyWrapUp Report of given week already exist");
                return BadRequest(response);
            }

            await WeeklyWrapUpService.AddWeeklyWrapUp(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] WeeklyWrapUpViewModel model)
        {
            if (WeeklyWrapUpService.IsWeeklyReportAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "WeeklyWrapUp Report of given week already exist");
                return BadRequest(response);
            }

            await WeeklyWrapUpService.UpdateWeeklyWrapUp(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await WeeklyWrapUpService.DeleteWeeklyWrapUp(id);
            return Ok();
        }
    }

}
