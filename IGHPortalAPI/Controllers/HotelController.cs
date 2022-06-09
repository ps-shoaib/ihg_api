using IGHportalAPI.Error;
using IGHportalAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGHportalAPI.ViewModels.HotelViewModels;
using System.Security.Claims;

namespace IGHportalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> logger;
        private readonly IHotelService hotelService;

        public HotelController(ILogger<HotelController> logger, IHotelService hotelService)
        {
            this.logger = logger;
            this.hotelService = hotelService;
        }


        [HttpGet("AllHotelsNames")]
        public IActionResult GetHotelsNames()
        {
            var Hotels = hotelService.GetHotelsNames();
            return Ok(Hotels);
        }



        [HttpGet("GetHotelByUser")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            
            var UserClaims =    this.User.Claims.FirstOrDefault();
            

            var Hotels = await hotelService.GetHotelsByUserId(userId, Request);
            return Ok(Hotels);
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Hotels = await hotelService.GetHotels(Request);
            return Ok(Hotels);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var Hotel = hotelService.GetHotel(id,Request);
            return Ok(Hotel);
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromForm] HotelViewModel model)
        {
            if (hotelService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Hotel with given name already exist try with different name");

                return BadRequest(response);
            }

            await hotelService.AddHotel(model);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] HotelViewModel model)
        {

            if (hotelService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Hotel with given name already exist try with different name");

                return BadRequest(response);
            }

            await hotelService.UpdateHotel(model);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await hotelService.DeleteHotel(id);
            return Ok();
        }
    }

}
