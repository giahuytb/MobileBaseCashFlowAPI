using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Repository;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameModeService : GameModeRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameModeService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            var mode = await (from gm in _context.GameModes
                              select new
                              {
                                  gm.GameModeId,
                                  gm.ImageUrl,
                                  gm.Description,
                                  gm.CreateAt,
                                  gm.CreateBy,
                                  gm.UpdateAt,
                                  gm.UpdateBy,
                              }).AsNoTracking().ToListAsync();
            return mode;
        }

        public async Task<object?> GetAsync(int gameModeId)
        {
            var mode = await (from gm in _context.GameModes
                              where gm.GameModeId == gameModeId
                              select new
                              {
                                  gm.GameModeId,
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
            var checkName = await _context.GameModes.Where(gm => gm.ModeName == request.ModeName).FirstOrDefaultAsync();
            if (checkName != null)
            {
                return "This mode name is already existed";
            }
            var gameMode = new GameMode()
            {
                ModeName = request.ModeName,
                ImageUrl = request.ImageUrl,
                Description = request.Description,
                CreateAt = DateTime.Now,
                CreateBy = userId,
            };

            await _context.GameModes.AddAsync(gameMode);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int gameModeId, int userId, GameModeRequest request)
        {

            var oldGameMode = await _context.GameModes.Where(g => g.GameModeId == gameModeId).FirstOrDefaultAsync();
            if (oldGameMode != null)
            {


                oldGameMode.ModeName = request.ModeName;
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
            var gameMode = await _context.GameModes.Where(g => g.GameModeId == gameModeId).FirstOrDefaultAsync();
            if (gameMode != null)
            {
                _context.GameModes.Remove(gameMode);
            }
            return "Can not found this game mode";
        }

    }
}
