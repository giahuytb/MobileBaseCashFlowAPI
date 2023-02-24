using Microsoft.EntityFrameworkCore;
using System.Collections;

using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;

namespace MobileBasedCashFlowAPI.Services
{
    public class BoardService : IBoardService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public BoardService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var board = await (from b in _context.Boards
                                   select new
                                   {
                                       b.BoardId,
                                       b.AmountFatTile,
                                       b.AmountRatTile,
                                       b.DementionBoard,
                                       b.RadiusRatTile,
                                       b.CreateAt,
                                   }).ToListAsync();
                return board;
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
                var board = await _context.Boards
                    .Select(b => new
                    {
                        b.BoardId,
                        b.AmountFatTile,
                        b.AmountRatTile,
                        b.DementionBoard,
                        b.RadiusRatTile,
                        b.CreateAt,
                    })
                    .Where(b => b.BoardId == id)
                    .FirstOrDefaultAsync();
                if (board != null)
                {
                    return board;
                }
                return null;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string userId, BoardRequest board)
        {
            try
            {
                var gameId = await (from game in _context.Games
                                    where game.GameVersion == "Ver_1"
                                    select new { gameId = game.GameId }).FirstOrDefaultAsync();

                if (gameId == null)
                {
                    return "can not find this game version";
                }
                else if (!ValidateInput.isNumber(board.AmountFatTile.ToString()) || board.AmountFatTile <= 0)
                {
                    return "Amount fat tile must be mumber and bigger than 0";
                }
                else if (!ValidateInput.isNumber(board.AmountRatTile.ToString()) || board.AmountRatTile <= 0)
                {
                    return "Amount rate tile must be mumber and bigger than 0";
                }
                var board1 = new Board()
                {
                    BoardId = Guid.NewGuid().ToString(),
                    AmountFatTile = board.AmountFatTile,
                    AmountRatTile = board.AmountRatTile,
                    DementionBoard = board.DementionBoard,
                    RadiusRatTile = board.RadiusRatTile,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                    GameId = gameId.gameId,
                };

                _context.Boards.Add(board1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string boardId, string userId, BoardRequest board)
        {
            var oldBoard = await _context.Boards.Where(i => i.BoardId == boardId).FirstOrDefaultAsync();
            if (oldBoard != null)
            {
                try
                {
                    if (!ValidateInput.isNumber(board.AmountFatTile.ToString()) || board.AmountFatTile <= 0)
                    {
                        return "Amount fat tile must be mumber and bigger than 0";
                    }
                    else if (!ValidateInput.isNumber(board.AmountRatTile.ToString()) || board.AmountRatTile <= 0)
                    {
                        return "Amount rate tile must be mumber and bigger than 0";
                    }
                    oldBoard.AmountFatTile = board.AmountFatTile;
                    oldBoard.AmountRatTile = board.AmountRatTile;
                    oldBoard.DementionBoard = board.DementionBoard;
                    oldBoard.RadiusRatTile = board.RadiusRatTile;
                    oldBoard.UpdateAt = DateTime.Now;
                    oldBoard.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this board";
        }

        public async Task<string> DeleteAsync(string boardId)
        {
            var board = await _context.Boards.FindAsync(boardId);
            if (board != null)
            {
                _context.Boards.Remove(board);
                await _context.SaveChangesAsync();

                return SUCCESS;
            }
            return "Can not find this board";
        }

    }
}
