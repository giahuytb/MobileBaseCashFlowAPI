
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameRepository(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            var gameRoom = await (from g in _context.Games
                                  select new
                                  {
                                      g.GameId,
                                      g.GameName,
                                      g.Description,
                                      g.CreateAt,
                                  }).AsNoTracking().ToListAsync();
            return gameRoom;
        }
        public async Task<object?> GetAsync(int id)
        {
            var gameRoom = await _context.Games
                .Select(g => new
                {
                    g.GameId,
                    g.GameName,
                    g.Description,
                    g.CreateAt,
                })
                .Where(b => b.GameId == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return gameRoom;
        }
        public async Task<string> CreateAsync(int userId, GameRequest request)
        {
            var checkName = await _context.Games
                        .Where(a => a.GameName == request.GameName)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
            if (checkName != null)
            {
                return "This game name is existed";
            }
            var gameRoom = new Game()
            {
                GameName = request.GameName,
                Description = request.Description,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                GameId = 1,
            };

            await _context.Games.AddAsync(gameRoom);
            await _context.SaveChangesAsync();
            return Constant.Success;

        }
        public async Task<string> UpdateAsync(int gameId, int userId, GameRequest request)
        {
            var oldGame = await _context.Games.Where(i => i.GameId == gameId).FirstOrDefaultAsync();
            if (oldGame != null)
            {
                var checkName = await _context.Games
                        .Where(a => a.GameName == request.GameName && a.GameName != oldGame.GameName)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This game name is existed";
                }
                oldGame.GameName = request.GameName;
                oldGame.Description = request.Description;
                oldGame.UpdateAt = DateTime.Now;
                oldGame.UpdateBy = userId;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this game room";
        }

        public async Task<string> DeleteAsync(int gameId)
        {
            var game = await _context.Games.Where(g => g.GameId == gameId).FirstOrDefaultAsync();
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();

                return Constant.Success;
            }
            return "Can not find this game room";
        }


    }
}
