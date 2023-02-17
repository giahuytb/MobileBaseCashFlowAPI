using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardsController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("board")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _boardService.GetAsync();
                if (result == null)
                {
                    return NotFound("list is empty");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("item/{name}")]
        public async Task<ActionResult<Item>> GetByName(string name)
        {
            try
            {
                var result = await _boardService.GetAsync(name);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("list is empty");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("item")]
        public async Task<ActionResult> PostBoard(BoardRequest board)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found , please login again ");
                }
                var result = await _boardService.CreateAsync(userId, board);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("board")]
        public async Task<ActionResult> UpdateBoard(string id, BoardRequest board)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login again");
                }
                var result = await _boardService.UpdateAsync(id, userId, board);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("board")]
        public async Task<ActionResult> DeleteBoard(string id)
        {
            try
            {
                var result = await _boardService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
