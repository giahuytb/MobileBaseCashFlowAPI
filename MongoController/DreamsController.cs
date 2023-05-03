
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.MongoServices;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DreamsController : ControllerBase
    {
        private readonly IDreamRepository _dreamService;
        public DreamsController(IDreamRepository dreamService)
        {
            _dreamService = dreamService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all dream")]
        public async Task<ActionResult<List<Dream?>>> GetAll()
        {
            var result = await _dreamService.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get list dream by paging and search")]
        public async Task<ActionResult<List<Dream>>> GetByPaging([FromQuery] PaginationFilter filter, double? from, double? to)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var validFilter = new PaginationFilter(filter.PageIndex, filter.PageSize);
            var result = await _dreamService.GetAsync(validFilter, from, to);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id:length(24)}")]
        [SwaggerOperation(Summary = "Get list dream by dream id")]
        public async Task<ActionResult<Dream>> GetById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _dreamService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this dream");
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create new dream")]
        public async Task<ActionResult> CreateDream(DreamRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _dreamService.CreateAsync(request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id:length(24)}")]
        [SwaggerOperation(Summary = "Update an existing dream")]
        public async Task<ActionResult<List<Dream>>> UpdateDream(string id, DreamRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _dreamService.UpdateAsync(id, request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            else if (result.Equals(Constant.NotFound))
            {
                return NotFound("Can not found this dream");
            }
            return BadRequest(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("inactive/{id:length(24)}")]
        [SwaggerOperation(Summary = "Inactive an existing dream")]
        public async Task<ActionResult> InActiveDream(string id)
        {
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _dreamService.InActiveAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            else if (result.Equals(Constant.NotFound))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:length(24)}")]
        [SwaggerOperation(Summary = "Delete an dream")]
        public async Task<ActionResult<List<Dream>>> DeleteDream(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _dreamService.RemoveAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            else if (result.Equals(Constant.NotFound))
            {
                return NotFound("Can not found this dream");
            }
            return BadRequest(result);
        }



    }
}
