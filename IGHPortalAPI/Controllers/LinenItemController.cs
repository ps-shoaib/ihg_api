using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.LinenItemViewModels;
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
    public class LinenItemController : ControllerBase
    {
        private readonly ILogger<LinenItemController> logger;
        private readonly ILinenItemService linenItemService;

        public LinenItemController(ILogger<LinenItemController> logger, ILinenItemService linenItemService)
        {
            this.logger = logger;
            this.linenItemService = linenItemService;
        }


        [HttpGet]
        public IActionResult Get(int hotelId)
        {
            var LinenItems = linenItemService.GetLinenItems(hotelId);
            return Ok(LinenItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var LinenItem = linenItemService.GetLinenItem(id);
            return Ok(LinenItem);
        }

        



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LinenItemViewModel model)
        {
            if (linenItemService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "LinenItem with given name already exist try with different name");

                return BadRequest(response);
            }

            await linenItemService.AddLinenItem(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LinenItemViewModel model)
        {

            if (linenItemService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "LinenItem with given name already exist try with different name");

                return BadRequest(response);
            }

            await linenItemService.UpdateLinenItem(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await linenItemService.DeleteLinenItem(id);
            return Ok();
        }
    }

}
