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
        public const string FAILED = "failed";
        public const string NOTFOUND = "not found";
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
                                       boardId = b.BoardId,
                                       amountFatTile = b.AmountFatTile,
                                       amountRateTile = b.AmountRatTile,
                                       dementionBoard = b.DementionBoard,
                                       radiusRatTile = b.RadiusRatTile,
                                       createAt = b.CreateAt,
                                   }).ToListAsync();
                return board;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<Object?> GetAsync(string id)
        {
            try
            {
                var board = await _context.Boards
                    .Select(b => new
                    {
                        boardId = b.BoardId,
                        amountFatTile = b.AmountFatTile,
                        amountRateTile = b.AmountRatTile,
                        dementionBoard = b.DementionBoard,
                        radiusRatTile = b.RadiusRatTile,
                        createAt = b.CreateAt,
                    })
                .Where(b => b.boardId == id)
                .FirstOrDefaultAsync();
                return board;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string userId, BoardRequest board)
        {
            var gameId = await (from game in _context.Games
                                where game.GameVersion == board.GameVersion
                                select new { gameId = game.GameId }).FirstOrDefaultAsync();

            if (gameId == null)
            {
                return "can not find this game versuin";
            }
            else if (!ValidateInput.isNumber(board.AmountFatTile.ToString()) || board.AmountFatTile <= 0)
            {
                return "Amount fat tile must be mumber and bigger than 0";
            }
            else if (!ValidateInput.isNumber(board.AmountRatTile.ToString()) || board.AmountRatTile <= 0)
            {
                return "Amount rate tile must be mumber and bigger than 0";
            }

            try
            {
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
            var oldBoard = await _context.Boards.FirstOrDefaultAsync(i => i.BoardId == boardId);
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
                    if (!BoardExists(boardId))
                    {
                        return NOTFOUND;
                    }
                    return ex.ToString();
                }
            }
            return FAILED;
        }

        public async Task<string> DeleteAsync(string boardId)
        {
            var board = await _context.Boards.FindAsync(boardId);
            if (board == null)
            {
                return NOTFOUND;
            }
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

        private bool BoardExists(string id)
        {
            return _context.Boards.Any(e => e.BoardId == id);
        }
    }
}
