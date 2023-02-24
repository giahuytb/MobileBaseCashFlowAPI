using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class TileTypeService : ITileTypeService
    {
        public const string SUCCESS = "success";

        private readonly MobileBasedCashFlowGameContext _context;

        public TileTypeService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var tiletype = await (from t in _context.TileTypes
                                      select new
                                      {
                                          t.TileTypeId,
                                          t.TileTypeName,
                                          t.CreateAt,
                                      }).ToListAsync();
                return tiletype;
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
                var board = await _context.TileTypes
                    .Select(t => new
                    {
                        t.TileTypeId,
                        t.TileTypeName,
                        t.CreateAt,
                    })
                    .Where(t => t.TileTypeId == id)
                    .FirstOrDefaultAsync();
                return board;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CreateAsync(string userId, TileTypeRequest tileType)
        {
            try
            {
                var tiletype1 = new TileType()
                {
                    TileTypeId = Guid.NewGuid().ToString(),
                    TileTypeName = tileType.TileTypeName,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                };

                _context.TileTypes.Add(tiletype1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<string> UpdateAsync(string tileTypeId, string userId, TileTypeRequest tileType)
        {
            var oldTileType = await _context.TileTypes.FirstOrDefaultAsync(i => i.TileTypeId == tileTypeId);
            if (oldTileType != null)
            {
                try
                {
                    oldTileType.TileTypeName = tileType.TileTypeName;
                    oldTileType.UpdateAt = DateTime.Now;
                    oldTileType.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this tile type";
        }

        public async Task<string> DeleteAsync(string tileTypeId)
        {
            var tile = await _context.Tiles.FindAsync(tileTypeId);
            if (tile == null)
            {
                return "Can not find this tile type";
            }
            _context.Tiles.Remove(tile);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

    }
}
