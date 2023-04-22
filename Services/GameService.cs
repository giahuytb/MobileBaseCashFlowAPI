
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameService : GameRepository
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public GameService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var game = await (from g in _context.Games
                                  select new
                                  {
                                      g.GameId,
                                      g.RoomNumber,
                                      g.RoomName,
                                      g.CreateAt,
                                  }).ToListAsync();
                return game;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<object?> GetAsync(int id)
        {
            try
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
                    .FirstOrDefaultAsync();
                return game;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CreateAsync(int userId, GameRequest gameRequest)
        {
            try
            {
                var game1 = new Game()
                {
                    RoomName = gameRequest.RoomName,
                    RoomNumber = gameRequest.RoomNumber,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                    GameServerId = 1,
                };

                _context.Games.Add(game1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<string> UpdateAsync(int gameId, int userId, GameRequest gameRequest)
        {
            var oldGame = await _context.Games.FirstOrDefaultAsync(i => i.GameId == gameId);
            if (oldGame != null)
            {
                try
                {
                    oldGame.RoomName = gameRequest.RoomName;
                    oldGame.RoomNumber = gameRequest.RoomNumber;
                    oldGame.UpdateAt = DateTime.Now;
                    oldGame.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this game";
        }

        public async Task<string> DeleteAsync(string gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null)
            {
                return "Can not find this game";
            }
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

        public Task<string> DeleteAsync(int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
