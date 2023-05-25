using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.IRepositories;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class GameModRepository : IGameModRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameModRepository(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAllAsync()
        {
            var mode = await (from gm in _context.GameMods
                              select new
                              {
                                  gm.GameModId,
                                  gm.ImageUrl,
                                  gm.Description,
                                  gm.CreateAt,
                                  gm.CreateBy,
                                  gm.UpdateAt,
                                  gm.UpdateBy,
                              }).AsNoTracking().ToListAsync();
            return mode;
        }

        public async Task<object?> GetByIdAsync(int gameModId)
        {
            var mode = await (from gm in _context.GameMods
                              where gm.GameModId == gameModId
                              select new
                              {
                                  gm.GameModId,
                                  gm.ImageUrl,
                                  gm.Description,
                                  gm.CreateAt,
                                  gm.CreateBy,
                                  gm.UpdateAt,
                                  gm.UpdateBy,
                              }).AsNoTracking().ToListAsync();
            return mode;
        }

        public async Task<string> CreateAsync(int userId, GameModeRequest request)
        {
            var checkName = await _context.GameMods
                .Where(gm => gm.ModName == request.ModeName)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (checkName != null)
            {
                return "This mode name is already existed";
            }
            var gameMode = new GameMod()
            {
                ModName = request.ModeName,
                ImageUrl = request.ImageUrl,
                Description = request.Description,
                CreateAt = DateTime.Now,
                CreateBy = userId,
            };

            await _context.GameMods.AddAsync(gameMode);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int gameModeId, int userId, GameModeRequest request)
        {

            var oldGameMode = await _context.GameMods.Where(g => g.GameModId == gameModeId).FirstOrDefaultAsync();
            if (oldGameMode != null)
            {
                var checkName = await _context.GameMods
                    .Where(gm => gm.ModName == request.ModeName && gm.ModName != oldGameMode.ModName)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This mode name is already existed";
                }
                oldGameMode.ModName = request.ModeName;
                oldGameMode.ImageUrl = request.ImageUrl;
                oldGameMode.Description = request.Description;
                oldGameMode.UpdateBy = userId;
                oldGameMode.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return Constant.Success;
            };
            return "Can not found this game mode";
        }

        public async Task<string> DeleteAsync(int gameModeId)
        {
            var gameMode = await _context.GameMods.Where(g => g.GameModId == gameModeId).FirstOrDefaultAsync();
            if (gameMode != null)
            {
                _context.GameMods.Remove(gameMode);
            }
            return "Can not found this game mode";
        }

    }
}
