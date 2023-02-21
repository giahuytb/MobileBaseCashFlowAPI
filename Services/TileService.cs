﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            try
            {
                var tile1 = new Tile()
                {
                    TileId = Guid.NewGuid().ToString(),
                    IsRatRace = tile.IsRatRace,
                    EventId = tile.TileTypeId,
                    DreamId = tile.DreamId,
                    TileTypeId = tile.TileTypeId,
                    BoardId = tile.BoardId,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                };

                _context.Tiles.Add(tile1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string tileId, string userId, TileRequest tile)
        {
            var oldTile = await _context.Tiles.FirstOrDefaultAsync(i => i.TileId == tileId);
            if (oldTile != null)
            {
                try
                {
                    oldTile.IsRatRace = tile.IsRatRace;
                    oldTile.BoardId = tile.BoardId;
                    oldTile.EventId = tile.EventId;
                    oldTile.BoardId = tile.BoardId;
                    oldTile.TileTypeId = tile.TileTypeId;
                    oldTile.UpdateAt = DateTime.Now;
                    oldTile.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    if (!TileExists(tileId))
                    {
                        return NOTFOUND;
                    }
                    return ex.ToString();
                }
            }
            return FAILED;
        }

        public async Task<string> DeleteAsync(string tileId)
        {
            var tile = await _context.Tiles.FindAsync(tileId);
            if (tile == null)
            {
                return NOTFOUND;
            }
            _context.Tiles.Remove(tile);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }
        private bool TileExists(string id)
        {
            return _context.Tiles.Any(e => e.TileId == id);
        }

    }
}
