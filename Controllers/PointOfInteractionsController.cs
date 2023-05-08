using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.IRepositories;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointOfInteractionsController : ControllerBase
    {
        private readonly IPOIRepository _IPOIRepository;

        public PointOfInteractionsController(IPOIRepository IPOIRepository)
        {
            _IPOIRepository = IPOIRepository;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all Point Of Interaction")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _IPOIRepository.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get by Point Of Interaction id")]
        public async Task<ActionResult<PointOfInteraction>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _IPOIRepository.GetByIdAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this POI");
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create new Point Of Interaction")]
        public async Task<ActionResult> PostAsset(POIRequest request)
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
            var result = await _IPOIRepository.CreateAsync(Int32.Parse(userId), request);

            return Ok(result);
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Point Of Interaction")]
        public async Task<ActionResult> UpdateAsset(int id, POIRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _IPOIRepository.UpdateAsync(id, Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing Point Of Interaction")]
        public async Task<ActionResult> DeleteAsset(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _IPOIRepository.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
