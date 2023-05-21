﻿using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;
using System.Collections;
using MobileBasedCashFlowAPI.Common;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class GameMatchRepository : IGameMatchRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameMatchRepository(MobileBasedCashFlowGameContext context)
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
                                       match.GameModId,
                                   }).AsNoTracking().ToListAsync();
            return gameMatch;
        }
        public Task<int> TotalMatchInDay()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalMatchInWeek()
        {
            throw new NotImplementedException();
        }


        public async Task<object?> GetAsync(string matchId)
        {
            var gameMatch = await (from match in _context.GameMatches
                                   where match.MatchId == matchId
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
                                       match.GameModId,
                                   }).AsNoTracking().ToListAsync();
            return gameMatch;
        }

        public async Task<string> CreateAsync(int userId, GameMatchRequest request)
        {
            var checkGameId = _context.Games.Where(gr => gr.GameId == request.gameModId).AsNoTracking().FirstOrDefault();
            if (checkGameId == null)
            {
                return "Can not found this game";
            }
            var match = new GameMatch()
            {
                MatchId = Guid.NewGuid().ToString(),
                MaxNumberPlayer = request.MaxNumberPlayer,
                WinnerId = request.WinnerId,
                HostId = userId,
                LastHostId = request.LastHostId,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                TotalRound = request.TotalRound,
                GameModId = request.gameModId,
            };

            await _context.GameMatches.AddAsync(match);
            await _context.SaveChangesAsync();
            return match.MatchId;

        }
        public async Task<string> UpdateAsync(string matchId, GameMatchRequest request)
        {
            var oldMatch = await _context.GameMatches.Where(gm => gm.MatchId == matchId).FirstOrDefaultAsync();
            if (oldMatch != null)
            {
                oldMatch.WinnerId = request.WinnerId;
                oldMatch.LastHostId = request.LastHostId;
                oldMatch.TotalRound = request.TotalRound;
                oldMatch.EndTime = DateTime.Now;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this match";
        }

        public async Task<string> DeleteAsync(string matchId)
        {
            var match = await _context.GameMatches.Where(gm => gm.MatchId == matchId).FirstOrDefaultAsync();
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
