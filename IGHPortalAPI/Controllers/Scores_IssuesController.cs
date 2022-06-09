using IGHportalAPI.Error;
using IGHportalAPI.Services;
using IGHportalAPI.ViewModels.Scores_IssuesViewModels;
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
    public class Scores_IssuesController : ControllerBase
    {
        private readonly ILogger<Scores_IssuesController> logger;
        private readonly IScores_IssuesService linenItemService;

        public Scores_IssuesController(ILogger<Scores_IssuesController> logger, IScores_IssuesService linenItemService)
        {
            this.logger = logger;
            this.linenItemService = linenItemService;
        }


        [HttpGet]
        //public IActionResult Get(int hotelId)

        public IActionResult Get()
        {
            var Scores_Issuess = linenItemService.GetScores_Issues();
            return Ok(Scores_Issuess);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var Scores_Issues = linenItemService.GetScores_Issue(id);
            return Ok(Scores_Issues);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Scores_IssuesViewModel model)
        {
            if (linenItemService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Scores_Issue with given name already exist try with different name");

                return BadRequest(response);
            }

            await linenItemService.AddScores_Issues(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Scores_IssuesViewModel model)
        {

            if (linenItemService.SystemNameAlreadyExist(model))
            {
                APIResponse response = new APIResponse(400, "Scores_Issues with given name already exist try with different name");

                return BadRequest(response);
            }

            await linenItemService.UpdateScores_Issue(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await linenItemService.DeleteScores_Issue(id);
            return Ok();
        }
    }

}
