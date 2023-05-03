using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Repository;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameService : IGameRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            var game = await (from g in _context.Games
                              join createBy in _context.UserAccounts on g.CreateBy equals createBy.UserId
                              join updateBy in _context.UserAccounts on g.UpdateBy equals updateBy.UserId
                              select new
                              {
                                  g.GameId,
                                  g.GameVersion,
                                  createBy = createBy.NickName,
                                  updateBy = updateBy.NickName,
                              }).AsNoTracking().ToListAsync();
            return game;
        }

        public async Task<object?> GetAsync(int gameId)
        {
            var game = await (from g in _context.Games
                              join createBy in _context.UserAccounts on g.CreateBy equals createBy.UserId
                              join updateBy in _context.UserAccounts on g.UpdateBy equals updateBy.UserId
                              where g.GameId == gameId
                              select new
                              {
                                  g.GameId,
                                  g.GameVersion,
                                  createBy = createBy.NickName,
                                  updateBy = updateBy.NickName,
                              }).ToListAsync();
            return game;
        }
        public async Task<string> CreateAsync(int userId, GameRequest request)
        {
            var gameServer = new Game()
            {
                GameVersion = request.GameVersion,
                CreateAt = DateTime.Now,
                CreateBy = userId,
            };

            await _context.Games.AddAsync(gameServer);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int gameId, int userId, GameRequest request)
        {
            var oldGame = await _context.Games.Where(i => i.GameId == gameId).FirstOrDefaultAsync();
            if (oldGame != null)
            {
                var checkName = await _context.Games
                        .Where(a => a.GameVersion == request.GameVersion && a.GameVersion != oldGame.GameVersion)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This version is existed";
                }
                oldGame.GameVersion = request.GameVersion;
                oldGame.UpdateAt = DateTime.Now;
                oldGame.UpdateBy = userId;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this game";
        }

        public async Task<string> DeleteAsync(int gameId)
        {
            var gameServer = await _context.Games.Where(gs => gs.GameId == gameId).FirstOrDefaultAsync();
            if (gameServer != null)
            {
                _context.Games.Remove(gameServer);
                await _context.SaveChangesAsync();

                return Constant.Success;
            }
            return "Can not find this game";
        }


    }
}
