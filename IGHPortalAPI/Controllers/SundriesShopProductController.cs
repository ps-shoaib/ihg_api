using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.SundriesShopProductViewModels;
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
    public class SundriesShopProductController : ControllerBase
    {
        private readonly ILogger<SundriesShopProductController> logger;
        private readonly ISundriesShopProductService sundriesShopProductService;

        public SundriesShopProductController(ILogger<SundriesShopProductController> logger, ISundriesShopProductService sundriesShopProductService)
        {
            this.logger = logger;
            this.sundriesShopProductService = sundriesShopProductService;
        }


        [HttpGet]
        public IActionResult Get(int hotelId)
        {
            var SundriesShopProducts = sundriesShopProductService.GetSundriesShopProducts(hotelId);
            return Ok(SundriesShopProducts);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var SundriesShopProduct = sundriesShopProductService.GetSundriesShopProduct(id);
            return Ok(SundriesShopProduct);
        }

        



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SundriesShopProductViewModel model)
        {
            if (sundriesShopProductService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Product with given name already exist try with different name");

                return BadRequest(response);
            }

            await sundriesShopProductService.AddSundriesShopProduct(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SundriesShopProductViewModel model)
        {
            if (sundriesShopProductService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Product with given name already exist try with different name");

                return BadRequest(response);
            }


            await sundriesShopProductService.UpdateSundriesShopProduct(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await sundriesShopProductService.DeleteSundriesShopProduct(id);
            return Ok();
        }
    }

}
