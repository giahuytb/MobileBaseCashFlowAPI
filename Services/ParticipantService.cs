
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Repository;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class ParticipantService : IParticipantRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public ParticipantService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            var participant = await (from p in _context.Participants
                                     join u in _context.UserAccounts on p.UserId equals u.UserId
                                     select new
                                     {
                                         u.NickName,
                                         p.MatchId,
                                         p.CreateAt,
                                     }).AsNoTracking().ToListAsync();
            return participant;
        }

        public async Task<object?> GetAsync(int userId, string matchId)
        {
            var participant = await (from p in _context.Participants
                                     join u in _context.UserAccounts on p.UserId equals u.UserId
                                     where p.MatchId == matchId && p.UserId == userId
                                     select new
                                     {
                                         u.NickName,
                                         p.MatchId,
                                         p.CreateAt,
                                     }).AsNoTracking().ToListAsync();
            return participant;
        }

        public async Task<string> CreateAsync(ParticipantRequest request)
        {
            var participant = new Participant()
            {
                UserId = request.UserId,
                MatchId = request.MatchId,
                CreateAt = DateTime.Now,


            };
            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> DeleteAsync(int userId, string matchId)
        {
            var participant = await _context.Participants.Where(p => p.UserId == userId && p.MatchId == matchId).FirstOrDefaultAsync();
            if (participant != null)
            {
                _context.Participants.Remove(participant);
                return Constant.Success;
            }
            return Constant.NotFound;
        }

    }
}
