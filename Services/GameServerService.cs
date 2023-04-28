using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Repository;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameServerService : GameServerRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameServerService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            var gameServer = await (from g in _context.GameServers
                                    join createBy in _context.UserAccounts on g.CreateBy equals createBy.UserId
                                    join updateBy in _context.UserAccounts on g.UpdateBy equals updateBy.UserId
                                    select new
                                    {
                                        g.GameServerId,
                                        g.GameVersion,
                                        createBy = createBy.NickName,
                                        updateBy = updateBy.NickName,
                                    }).AsNoTracking().ToListAsync();
            return gameServer;
        }

        public async Task<object?> GetAsync(int gameServerId)
        {
            var gameServer = await (from g in _context.GameServers
                                    join createBy in _context.UserAccounts on g.CreateBy equals createBy.UserId
                                    join updateBy in _context.UserAccounts on g.UpdateBy equals updateBy.UserId
                                    where g.GameServerId == gameServerId
                                    select new
                                    {
                                        g.GameServerId,
                                        g.GameVersion,
                                        createBy = createBy.NickName,
                                        updateBy = updateBy.NickName,
                                    }).ToListAsync();
            return gameServer;
        }
        public async Task<string> CreateAsync(int userId, GameServerRequest request)
        {
            var gameServer = new GameServer()
            {
                GameVersion = request.GameVersion,
                CreateAt = DateTime.Now,
                CreateBy = userId,
            };

            await _context.GameServers.AddAsync(gameServer);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int gameServerId, int userId, GameServerRequest request)
        {
            var oldGameServer = await _context.GameServers.Where(i => i.GameServerId == gameServerId).FirstOrDefaultAsync();
            if (oldGameServer != null)
            {
                var checkName = await _context.GameServers
                        .Where(a => a.GameVersion == request.GameVersion && a.GameVersion != oldGameServer.GameVersion)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This version is existed";
                }
                oldGameServer.GameVersion = request.GameVersion;
                oldGameServer.UpdateAt = DateTime.Now;
                oldGameServer.UpdateBy = userId;

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
            return "Can not find this game server";
        }


    }
}
