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
using Microsoft.EntityFrameworkCore;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly MobileBasedCashFlowGameContext _context;

        public ParticipantsController(IParticipantRepository participantRepository, MobileBasedCashFlowGameContext context)
        {
            _participantRepository = participantRepository;
            _context = context;
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


        [HttpDelete("delete-all")]
        public async Task<ActionResult> DeleteAsset()
        {
            var allRecord = await _context.Participants.ToListAsync();
            _context.Participants.RemoveRange(allRecord);
            await _context.SaveChangesAsync();

            return Ok("Success");
        }
    }
}
