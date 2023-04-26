using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Services;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly ParticipantRepository _participantRepository;

        public ParticipantsController(ParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {

            var result = await _participantRepository.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpGet("{userId}/{matchId}")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<Participant>> GetById(int userId, int matchId)
        {
            var result = await _participantRepository.GetAsync(userId, matchId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this asset");
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<ActionResult> PostItem(ParticipantRequest request)
        {
            var result = await _participantRepository.CreateAsync(request);
            return Ok(result);
        }


        [HttpDelete("{userId}/{matchId}")]
        public async Task<ActionResult> DeleteAsset(int userId, int matchId)
        {
            var result = await _participantRepository.DeleteAsync(userId, matchId);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
