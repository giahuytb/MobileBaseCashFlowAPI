using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;

using System.Collections;
using MobileBasedCashFlowAPI.Common;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameMatchService : GameMatchRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameMatchService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            var gameMatch = await (from match in _context.GameMatches
                                   select new
                                   {
                                       match.MatchId,
                                       match.MaxNumberPlayer,
                                       match.WinnerId,
                                       match.HostId,
                                       match.LastHostId,
                                       match.StartTime,
                                       match.EndTime,
                                       match.TotalRound,
                                       match.GameId,
                                   }).AsNoTracking().ToListAsync();
            return gameMatch;
        }

        public async Task<object?> GetAsync(int id)
        {
            var gameMatch = await (from match in _context.GameMatches
                                   where match.MatchId == id
                                   select new
                                   {
                                       match.MatchId,
                                       match.MaxNumberPlayer,
                                       match.WinnerId,
                                       match.HostId,
                                       match.LastHostId,
                                       match.StartTime,
                                       match.EndTime,
                                       match.TotalRound,
                                       match.GameId,
                                   }).AsNoTracking().ToListAsync();
            return gameMatch;
        }

        public async Task<string> CreateAsync(int userId, int gameId, GameMatchRequest request)
        {
            var match = new GameMatch()
            {
                MaxNumberPlayer = request.MaxNumberPlayer,
                WinnerId = request.WinnerId,
                HostId = userId,
                LastHostId = request.LastHostId,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                TotalRound = request.TotalRound,
                GameId = gameId,
            };

            _context.GameMatches.Add(match);
            await _context.SaveChangesAsync();
            return Constant.Success;

        }
        public async Task<string> UpdateAsync(int gameMatchId, int userId, GameMatchRequest request)
        {
            var oldMatch = await _context.GameMatches.Where(gm => gm.MatchId == gameMatchId).FirstOrDefaultAsync();
            if (oldMatch != null)
            {
                oldMatch.WinnerId = request.WinnerId;
                oldMatch.HostId = userId;
                oldMatch.LastHostId = request.LastHostId;
                oldMatch.TotalRound = request.TotalRound;
                oldMatch.EndTime = DateTime.Now;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this match";
        }

        public async Task<string> DeleteAsync(int gameMatchId)
        {
            var match = await _context.GameMatches.Where(gm => gm.GameId == gameMatchId).FirstOrDefaultAsync();
            if (match != null)
            {
                _context.GameMatches.Remove(match);
                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this match";
        }


    }
}
