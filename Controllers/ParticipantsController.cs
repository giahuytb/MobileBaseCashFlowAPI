using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantRepository _participantRepository;

        public ParticipantsController(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all participant")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _participantRepository.GetAllAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{userId}/{matchId}")]
        [SwaggerOperation(Summary = "Get the participant with user id and match id")]
        public async Task<ActionResult<Participant>> GetById(int userId, string matchId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _participantRepository.GetByIdAsync(userId, matchId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this asset");
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create new participant")]
        public async Task<ActionResult> PostItem(ParticipantRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _participantRepository.CreateAsync(request);
            return Ok(result);
        }


        //[HttpDelete("{userId}/{matchId}")]
        //public async Task<ActionResult> DeleteAsset(int userId, int matchId)
        //{
        //    var result = await _participantRepository.DeleteAsync(userId, matchId);
        //    if (result.Equals(Constant.Success))
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}
    }
}
