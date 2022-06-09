using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.LinenInventoryViewModels;
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
    public class LinenInventoryController : ControllerBase
    {
        private readonly ILogger<LinenInventoryController> logger;
        private readonly ILinenInventoryService linenInventoryService;

        public LinenInventoryController(ILogger<LinenInventoryController> logger, ILinenInventoryService linenInventoryService)
        {
            this.logger = logger;
            this.linenInventoryService = linenInventoryService;
        }


        [HttpGet]
        public IActionResult GetAll(int hotelId, string Month = "", string Year = "")
        {
            var LinenInventorys = linenInventoryService.GetLinenInventories(hotelId, Month, Year);
            return Ok(LinenInventorys);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, int hotelId)
        {
            var LinenInventory = linenInventoryService.GetLinenInventory(id, hotelId);
            return Ok(LinenInventory);
        }

        



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LinenInventoryViewModel model)
        {
            if (linenInventoryService.ReportAlreadyExistOfCurrentMonth(model))
            {
                APIResponse response = new APIResponse(400, "Selected Month's  LinenInventory Report Already Exist! Please try after Deleting");

                return BadRequest(response);
                
            }


            await linenInventoryService.AddLinenInventory(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LinenInventoryViewModel model)
        {

            if (linenInventoryService.ReportAlreadyExistOfCurrentMonth(model))
            {
                APIResponse response = new APIResponse(400, "Selected Month's  LinenInventory Report Already Exist! Please try after Deleting");
                return BadRequest(response);
            }


            await linenInventoryService.UpdateLinenInventory(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await linenInventoryService.DeleteLinenInventory(id);
            return Ok();
        }
    }

}
