
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameReportsController : ControllerBase
    {
        private readonly GameReportRepository _gameReportService;

        public GameReportsController(GameReportRepository gameReportService)
        {
            _gameReportService = gameReportService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all game report")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameReportService.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get game report by game report id")]
        public async Task<ActionResult<GameReport>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameReportService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new game report")]
        public async Task<ActionResult> PostGameReport(GameReportRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // get the current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _gameReportService.CreateAsync(Int32.Parse(userId), request);
            return Ok(result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update an existing game report")]
        public async Task<ActionResult> UpdateGameReport(int gameReportId, GameReportRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameReportService.UpdateAsync(gameReportId, request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing game report")]
        public async Task<ActionResult> DeleteGameReport(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameReportService.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }


    }
}
