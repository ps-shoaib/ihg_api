using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.SundriesShopInventoryViewModels;
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
    public class SundriesShopInventoryController : ControllerBase
    {
        private readonly ILogger<SundriesShopInventoryController> logger;
        private readonly ISundriesShopInventoryService sundriesShopInventoryService;

        public SundriesShopInventoryController(ILogger<SundriesShopInventoryController> logger, ISundriesShopInventoryService sundriesShopInventoryService)
        {
            this.logger = logger;
            this.sundriesShopInventoryService = sundriesShopInventoryService;
        }


        [HttpGet]
        public IActionResult GetAll(int hotelId, string Month = "", string Year = "")
        {
            var SundriesShopInventorys = sundriesShopInventoryService.GetSundriesShopInventories(hotelId, Month, Year);
            return Ok(SundriesShopInventorys);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, int hotelId)
        {
            var SundriesShopInventory = sundriesShopInventoryService.GetSundriesShopInventory(id, hotelId);
            return Ok(SundriesShopInventory);
        }

        



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SundriesShopInventoryViewModel model)
        {
            if (sundriesShopInventoryService.ReportAlreadyExistOfCurrentMonth(model))
            {
                APIResponse response = new APIResponse(400, "Selected Month's  SundriesShopInventory Report Already Exist! Please try after Deleting");

                return BadRequest(response);

            }


            await sundriesShopInventoryService.AddSundriesShopInventory(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SundriesShopInventoryViewModel model)
        {
            if (sundriesShopInventoryService.ReportAlreadyExistOfCurrentMonth(model))
            {
                APIResponse response = new APIResponse(400, "Selected Month's  SundriesShopInventory Report Already Exist! Please try after Deleting");

                return BadRequest(response);

            }


            await sundriesShopInventoryService.UpdateSundriesShopInventory(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await sundriesShopInventoryService.DeleteSundriesShopInventory(id);
            return Ok();
        }
    }

}
