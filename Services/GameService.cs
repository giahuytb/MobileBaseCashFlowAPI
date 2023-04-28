
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameService : GameRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            var game = await (from g in _context.Games
                              select new
                              {
                                  g.GameId,
                                  g.RoomNumber,
                                  g.RoomName,
                                  g.CreateAt,
                              }).AsNoTracking().ToListAsync();
            return game;
        }
        public async Task<object?> GetAsync(int id)
        {
            var game = await _context.Games
                .Select(g => new
                {
                    g.GameId,
                    g.RoomNumber,
                    g.RoomName,
                    g.CreateAt,
                })
                .Where(b => b.GameId == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return game;
        }
        public async Task<string> CreateAsync(int userId, GameRequest request)
        {
            var game = new Game()
            {
                RoomName = request.RoomName,
                RoomNumber = request.RoomNumber,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                GameServerId = 1,
            };

            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return Constant.Success;

        }
        public async Task<string> UpdateAsync(int gameId, int userId, GameRequest request)
        {
            var oldGame = await _context.Games.Where(i => i.GameId == gameId).FirstOrDefaultAsync();
            if (oldGame != null)
            {
                var checkName = await _context.Games
                        .Where(a => a.RoomName == request.RoomName && a.RoomName != oldGame.RoomName)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This room name is existed";
                }
                oldGame.RoomName = request.RoomName;
                oldGame.RoomNumber = request.RoomNumber;
                oldGame.UpdateAt = DateTime.Now;
                oldGame.UpdateBy = userId;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this game";
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
            return "Can not find this game";
        }


    }
}
