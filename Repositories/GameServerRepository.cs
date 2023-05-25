using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.IRepositories;
using System.Collections;
using DnsClient;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class GameServerRepository : IGameServerRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameServerRepository(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAllAsync()
        {
            var game = await (from g in _context.GameServers
                              select new
                              {
                                  g.GameServerId,
                                  g.GameVersion,
                              }).AsNoTracking().ToListAsync();
            return game;
        }

        public async Task<object?> GetByIdAsync(int gameServerId)
        {
            var game = await (from g in _context.GameServers
                              where g.GameServerId == gameServerId
                              select new
                              {
                                  g.GameServerId,
                                  g.GameVersion,
                              }).ToListAsync();
            return game;
        }
        public async Task<string> CreateAsync(GameServerRequest request)
        {
            var checkVersion = await _context.GameServers
                .Where(gm => gm.GameVersion == request.GameVersion)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (checkVersion != null)
            {
                return "This version is already existed";
            }
            var gameServer = new GameServer()
            {
                GameVersion = request.GameVersion,
            };

            await _context.GameServers.AddAsync(gameServer);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int gameServerId, GameServerRequest request)
        {
            var oldGame = await _context.GameServers.Where(i => i.GameServerId == gameServerId).FirstOrDefaultAsync();
            if (oldGame != null)
            {
                var checkName = await _context.GameServers
                        .Where(a => a.GameVersion == request.GameVersion && a.GameVersion != oldGame.GameVersion)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This version is already existed";
                }
                oldGame.GameVersion = request.GameVersion;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this game";
        }

        public async Task<string> DeleteAsync(int gameServerId)
        {
            var gameServer = await _context.GameServers.Where(gs => gs.GameServerId == gameServerId).FirstOrDefaultAsync();
            if (gameServer != null)
            {
                _context.GameServers.Remove(gameServer);
                await _context.SaveChangesAsync();

                return Constant.Success;
            }
            return "Can not find this game";
        }


    }
}
