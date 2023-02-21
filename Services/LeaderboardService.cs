using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        public const string SUCCESS = "success";
        public const string FAILED = "failed";
        public const string NOTFOUND = "not found";
        private readonly MobileBasedCashFlowGameContext _context;
        public LeaderboardService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var leaderboard = await (from l in _context.Leaderboards
                                         join game in _context.Games
                                         on l.GameId equals game.GameId
                                         select new
                                         {
                                             leaderboardId = l.LeaderBoardId,
                                             timePeriod = l.TimeFeriod,
                                             timePeriodFrom = l.TimePeriodFrom,
                                             score = l.Score,
                                             gameVersion = game.GameVersion,
                                             createAt = l.CreateAt,
                                         }).ToListAsync();
                return leaderboard;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object?> GetAsync(string id)
        {
            try
            {
                var leaderboard = await (from l in _context.Leaderboards
                                         join game in _context.Games
                                         on l.GameId equals game.GameId
                                         where l.LeaderBoardId == id
                                         select new
                                         {
                                             leaderboardId = l.LeaderBoardId,
                                             timePeriod = l.TimeFeriod,
                                             timePeriodFrom = l.TimePeriodFrom,
                                             score = l.Score,
                                             gameVersion = game.GameVersion,
                                             createAt = l.CreateAt,
                                         }).FirstOrDefaultAsync();
                return leaderboard;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string userId, LeaderboardRequest leaderboard)
        {
            try
            {
                var gameId = await (from game in _context.Games
                                    where game.GameVersion == leaderboard.GameVersion
                                    select new { gameId = game.GameId }).FirstOrDefaultAsync();

                if (gameId == null)
                {
                    return "can not find this game version";
                }
                //else if (!ValidateInput.isNumber(board.AmountFatTile.ToString()) || board.AmountFatTile <= 0)
                //{
                //    return "Amount fat tile must be mumber and bigger than 0";
                //}
                //else if (!ValidateInput.isNumber(board.AmountRatTile.ToString()) || board.AmountRatTile <= 0)
                //{
                //    return "Amount rate tile must be mumber and bigger than 0";
                //}
                var leaderboard1 = new Leaderboard()
                {
                    LeaderBoardId = Guid.NewGuid().ToString(),
                    TimeFeriod = leaderboard.TimePeriod,
                    TimePeriodFrom = leaderboard.TimePeriodFrom,
                    Score = leaderboard.Score,
                    GameId = gameId.gameId,
                    CreateAt = DateTime.Now,
                };

                _context.Leaderboards.Add(leaderboard1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string leaderboardId, string userId, LeaderboardRequest leaderboard)
        {
            var oldLeaderboard = await _context.Leaderboards.FirstOrDefaultAsync(d => d.LeaderBoardId == leaderboardId);

            if (oldLeaderboard != null)
            {
                try
                {
                    var gameId = await (from game in _context.Games
                                        where game.GameVersion == leaderboard.GameVersion
                                        select new { gameId = game.GameId }).FirstOrDefaultAsync();

                    if (gameId == null)
                    {
                        return "can not find this game version";
                    }
                    oldLeaderboard.TimeFeriod = leaderboard.TimePeriod;
                    oldLeaderboard.TimePeriodFrom = leaderboard.TimePeriodFrom;
                    oldLeaderboard.Score = leaderboard.Score;
                    oldLeaderboard.GameId = gameId.gameId;
                    oldLeaderboard.PlayerId = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    if (!LeaderboardExists(leaderboardId))
                    {
                        return NOTFOUND;
                    }
                    return ex.ToString();
                }
            }
            return FAILED;
        }

        public async Task<string> DeleteAsync(string leaderboardId)
        {
            var leaderboard = await _context.Leaderboards.FindAsync(leaderboardId);
            if (leaderboard == null)
            {
                return NOTFOUND;
            }
            _context.Leaderboards.Remove(leaderboard);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

        public bool LeaderboardExists(string id)
        {
            return _context.Leaderboards.Any(d => d.LeaderBoardId == id);
        }

    }
}
