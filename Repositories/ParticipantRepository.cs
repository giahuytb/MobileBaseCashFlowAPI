
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.IRepositories;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public ParticipantRepository(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAllAsync()
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

        public async Task<object?> GetByIdAsync(int userId, string matchId)
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

        public async Task<int> GetTotalUserPlayGameInDay()
        {
            var total = await (from p in _context.Participants
                               join gm in _context.GameMatches on p.MatchId equals gm.MatchId
                               where gm.StartTime >= DateTime.Today && gm.StartTime <= DateTime.Today.AddDays(1)
                               select new
                               {
                                   p.UserId
                               }).Distinct().CountAsync();
            return total;
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
