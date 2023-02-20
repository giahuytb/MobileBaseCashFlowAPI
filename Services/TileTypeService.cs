using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class TileTypeService : ITileTypeService
    {
        public const string SUCCESS = "success";
        public const string FAILED = "failed";
        public const string NOTFOUND = "not found";
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
                                          tileTypeId = t.TileTypeId,
                                          tileTypeName = t.TileTypeName,
                                          createAt = t.CreateAt,
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
                        tileTypeId = t.TileTypeId,
                        tileTypeName = t.TileTypeName,
                        createAt = t.CreateAt,
                    })
                    .Where(t => t.tileTypeId == id)
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
                    if (!TileTypeExists(tileTypeId))
                    {
                        return NOTFOUND;
                    }
                    return ex.ToString();
                }
            }
            return FAILED;
        }
        public Task<string> DeleteAsync(string tileTypeId)
        {
            throw new NotImplementedException();
        }

        private bool TileTypeExists(string id)
        {
            return _context.TileTypes.Any(e => e.TileTypeId == id);
        }

    }
}
