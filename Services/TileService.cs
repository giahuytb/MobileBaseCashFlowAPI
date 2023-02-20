using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class TileService : ITileService
    {
        public const string SUCCESS = "success";
        public const string FAILED = "failed";
        public const string NOTFOUND = "not found";
        private readonly MobileBasedCashFlowGameContext _context;

        public TileService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var tiles = await (from tile in _context.Tiles
                                   join board in _context.Boards on tile.BoardId equals board.BoardId
                                   join dream in _context.Dreams on tile.DreamId equals dream.DreamId
                                   join evt in _context.GameEvents on tile.EventId equals evt.EventId
                                   join tileType in _context.TileTypes on tile.TileTypeId equals tileType.TileTypeId
                                   select new
                                   {
                                       tileId = tile.TileId,
                                       isRatRace = tile.IsRatRace,
                                       createAt = tile.CreateAt,
                                       board = board.DementionBoard,
                                       eventName = evt.EventName,
                                       dreamName = dream.DreamName,
                                       tileTypeName = tileType.TileTypeName,
                                   }).ToListAsync();
                return tiles;
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
                var tile1 = await (from tile in _context.Tiles
                                   join board in _context.Boards on tile.BoardId equals board.BoardId
                                   join dream in _context.Dreams on tile.DreamId equals dream.DreamId
                                   join evt in _context.GameEvents on tile.EventId equals evt.EventId
                                   join tileType in _context.TileTypes on tile.TileTypeId equals tileType.TileTypeId
                                   select new
                                   {
                                       tileId = tile.TileId,
                                       isRatRace = tile.IsRatRace,
                                       createAt = tile.CreateAt,
                                       board = board.DementionBoard,
                                       eventName = evt.EventName,
                                       dreamName = dream.DreamName,
                                       tileTypeName = tileType.TileTypeName,
                                   }).Where(tile => tile.tileId == id).FirstOrDefaultAsync();
                return tile1;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CreateAsync(string userId, TileRequest tile)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateAsync(string tileId, string userId, TileRequest tile)
        {
            throw new NotImplementedException();
        }

        public async Task<string> DeleteAsync(string tileId)
        {
            throw new NotImplementedException();
        }

    }
}
