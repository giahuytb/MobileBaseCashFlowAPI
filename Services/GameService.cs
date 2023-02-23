
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameService : IGameService
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
                                      gameId = g.GameId,
                                      gameVersion = g.GameVersion,
                                      backgroundImageUrl = g.BackgroundImageUrl,
                                      createAt = g.CreateAt,
                                  }).ToListAsync();
                return game;
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
                var game = await _context.Games
                    .Select(g => new
                    {
                        gameId = g.GameId,
                        gameVersion = g.GameVersion,
                        backgroundImageUrl = g.BackgroundImageUrl,
                        createAt = g.CreateAt,
                    })
                    .Where(b => b.gameId == id)
                    .FirstOrDefaultAsync();
                return game;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CreateAsync(GameRequest gameRequest)
        {
            try
            {
                var check = await _context.Games.Where(g => g.GameVersion == gameRequest.GameVersion).FirstOrDefaultAsync();

                if (check == null)
                {
                    return "This game version already Exist";
                }
                var game1 = new Game()
                {
                    GameId = Guid.NewGuid().ToString(),
                    GameVersion = gameRequest.GameVersion,
                    BackgroundImageUrl = gameRequest.BackgroundImageUrl,
                    CreateAt = DateTime.Now,
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
        public async Task<string> UpdateAsync(string gameId, GameRequest gameRequest)
        {
            var oldGame = await _context.Games.FirstOrDefaultAsync(i => i.GameId == gameId);
            if (oldGame != null)
            {
                try
                {
                    oldGame.GameVersion = gameRequest.GameVersion;
                    oldGame.BackgroundImageUrl = gameRequest.BackgroundImageUrl;
                    oldGame.UpdateAt = DateTime.Now;

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


    }
}
