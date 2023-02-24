
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameEventService : IGameEventService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public GameEventService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var gameEvent = await (from evt in _context.GameEvents
                                       select new
                                       {
                                           evt.EventId,
                                           evt.EventName,
                                           evt.IsEventTile,
                                           evt.CreateAt,
                                       }).ToListAsync();
                return gameEvent;
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
                var gameEvent = await _context.GameEvents
                    .Select(evt => new
                    {
                        evt.EventId,
                        evt.EventName,
                        evt.IsEventTile,
                        evt.CreateAt,
                    })
                    .Where(d => d.EventId == id)
                    .FirstOrDefaultAsync();
                return gameEvent;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string userId, GameEventRequest gameEvent)
        {
            try
            {
                var evt = new GameEvent()
                {
                    EventId = Guid.NewGuid().ToString(),
                    EventName = gameEvent.EventName,
                    IsEventTile = gameEvent.IsEventTile,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                };
                _context.GameEvents.Add(evt);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string eventId, string userId, GameEventRequest gameEvent)
        {
            var oldGameEvent = await _context.GameEvents.FirstOrDefaultAsync(d => d.EventId == eventId);
            if (oldGameEvent != null)
            {
                try
                {
                    oldGameEvent.EventName = gameEvent.EventName;
                    oldGameEvent.IsEventTile = gameEvent.IsEventTile;
                    oldGameEvent.UpdateAt = DateTime.Now;
                    oldGameEvent.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this game event";
        }

        public async Task<string> DeleteAsync(string eventId)
        {
            var gameEvent = await _context.GameEvents.FindAsync(eventId);
            if (gameEvent == null)
            {
                return "Can not find this game event";
            }
            _context.GameEvents.Remove(gameEvent);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

    }
}
