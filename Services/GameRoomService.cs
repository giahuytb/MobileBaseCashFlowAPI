
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameRoomService : IGameRoomRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameRoomService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            var gameRoom = await (from g in _context.GameRooms
                                  select new
                                  {
                                      g.GameId,
                                      g.RoomNumber,
                                      g.RoomName,
                                      g.CreateAt,
                                  }).AsNoTracking().ToListAsync();
            return gameRoom;
        }
        public async Task<object?> GetAsync(int id)
        {
            var gameRoom = await _context.GameRooms
                .Select(g => new
                {
                    g.GameId,
                    g.RoomNumber,
                    g.RoomName,
                    g.CreateAt,
                })
                .Where(b => b.GameId == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return gameRoom;
        }
        public async Task<string> CreateAsync(int userId, GameRoomRequest request)
        {
            var gameRoom = new GameRoom()
            {
                RoomName = request.RoomName,
                RoomNumber = request.RoomNumber,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                GameId = 1,
            };

            await _context.GameRooms.AddAsync(gameRoom);
            await _context.SaveChangesAsync();
            return Constant.Success;

        }
        public async Task<string> UpdateAsync(int gameId, int userId, GameRoomRequest request)
        {
            var oldGameRoom = await _context.GameRooms.Where(i => i.GameId == gameId).FirstOrDefaultAsync();
            if (oldGameRoom != null)
            {
                var checkName = await _context.GameRooms
                        .Where(a => a.RoomName == request.RoomName && a.RoomName != oldGameRoom.RoomName)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This room name is existed";
                }
                oldGameRoom.RoomName = request.RoomName;
                oldGameRoom.RoomNumber = request.RoomNumber;
                oldGameRoom.UpdateAt = DateTime.Now;
                oldGameRoom.UpdateBy = userId;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this game room";
        }

        public async Task<string> DeleteAsync(int gameId)
        {
            var gameRoom = await _context.GameRooms.Where(g => g.GameId == gameId).FirstOrDefaultAsync();
            if (gameRoom != null)
            {
                _context.GameRooms.Remove(gameRoom);
                await _context.SaveChangesAsync();

                return Constant.Success;
            }
            return "Can not find this game room";
        }


    }
}
