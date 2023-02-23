using Microsoft.EntityFrameworkCore;
using System.Collections;

using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;

namespace MobileBasedCashFlowAPI.Services
{
    public class EventCardService : IEventCardService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public EventCardService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var eventCard = await (from evc in _context.EventCards
                                       select new
                                       {
                                           eventId = evc.EventId,
                                           cardName = evc.CardName,
                                           cost = evc.Cost,
                                           downPay = evc.DownPay,
                                           dept = evc.Dept,
                                           cashFlow = evc.CashFlow,
                                           tradingRange = evc.TradingRange,
                                           description = evc.Description,
                                           eventImageUrl = evc.EventImageUrl,
                                           createAt = evc.CreateAt,
                                       }).ToListAsync();
                return eventCard;
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
                var eventCard = await _context.EventCards
                    .Select(evc => new
                    {
                        eventId = evc.EventId,
                        cardName = evc.CardName,
                        cost = evc.Cost,
                        downPay = evc.DownPay,
                        dept = evc.Dept,
                        cashFlow = evc.CashFlow,
                        tradingRange = evc.TradingRange,
                        description = evc.Description,
                        eventImageUrl = evc.EventImageUrl,
                        createAt = evc.CreateAt,
                    })
                    .Where(i => i.eventId == id)
                    .FirstOrDefaultAsync();
                return eventCard;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string userId, EventCardRequest eventCard)
        {
            var gameId = await (from game in _context.Games
                                where game.GameVersion == eventCard.GameVersion
                                select new { gameId = game.GameId })
                                .FirstOrDefaultAsync();

            var gameEventId = await (from gameEvt in _context.GameEvents
                                     where gameEvt.EventName == eventCard.GameEventName
                                     select new { eventId = gameEvt.EventId })
                                     .FirstOrDefaultAsync();

            if (gameId == null)
            {
                return "can not find this game version";
            }
            else if (gameEventId == null)
            {
                return "can not found this event name";
            }
            else if (!ValidateInput.isNumber(eventCard.Cost.ToString()) || eventCard.Cost <= 0)
            {
                return "Cost must be mumber and bigger than 0";
            }
            else if (!ValidateInput.isNumber(eventCard.DownPay.ToString()) || eventCard.DownPay <= 0)
            {
                return "DownPay must be mumber and bigger than 0";
            }
            else if (!ValidateInput.isNumber(eventCard.Dept.ToString()) || eventCard.Dept <= 0)
            {
                return "Dept must be mumber and bigger than 0";
            }
            else if (!ValidateInput.isNumber(eventCard.CashFlow.ToString()) || eventCard.CashFlow <= 0)
            {
                return "Cash Flow must be mumber and bigger than 0";
            }
            try
            {
                var eventCard1 = new EventCard()
                {
                    EventCardId = Guid.NewGuid().ToString(),
                    CardName = eventCard.CardName,
                    Cost = eventCard.Cost,
                    DownPay = eventCard.DownPay,
                    Dept = eventCard.Dept,
                    CashFlow = eventCard.CashFlow,
                    TradingRange = eventCard.TradingRange,
                    Description = eventCard.Description,
                    EventImageUrl = eventCard.EventImageUrl,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,

                    GameId = gameId.gameId,
                    EventId = gameEventId.eventId,
                };

                _context.EventCards.Add(eventCard1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string cardId, string userId, EventCardRequest eventCard)
        {
            var oldEventCard = await _context.EventCards.Where(d => d.EventCardId == cardId).FirstOrDefaultAsync();
            if (oldEventCard != null)
            {
                try
                {
                    if (!ValidateInput.isNumber(eventCard.Cost.ToString()) || eventCard.Cost <= 0)
                    {
                        return "Cost must be mumber and bigger than 0";
                    }
                    if (!ValidateInput.isNumber(eventCard.DownPay.ToString()) || eventCard.DownPay <= 0)
                    {
                        return "DownPay must be mumber and bigger than 0";
                    }
                    if (!ValidateInput.isNumber(eventCard.Dept.ToString()) || eventCard.Dept <= 0)
                    {
                        return "Dept must be mumber and bigger than 0";
                    }
                    if (!ValidateInput.isNumber(eventCard.CashFlow.ToString()) || eventCard.CashFlow <= 0)
                    {
                        return "Cash Flow must be mumber and bigger than 0";
                    }
                    oldEventCard.CardName = eventCard.CardName;
                    oldEventCard.Cost = eventCard.Cost;
                    oldEventCard.DownPay = eventCard.DownPay;
                    oldEventCard.Dept = eventCard.Cost;
                    oldEventCard.CashFlow = eventCard.CashFlow;
                    oldEventCard.TradingRange = eventCard.TradingRange;
                    oldEventCard.Description = eventCard.Description;
                    oldEventCard.EventImageUrl = eventCard.EventImageUrl;
                    oldEventCard.UpdateAt = DateTime.Now;
                    oldEventCard.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this event card";
        }

        public async Task<string> DeleteAsync(string cardId)
        {
            var eventCard = await _context.EventCards.FindAsync(cardId);
            if (eventCard == null)
            {
                return "Can not find this event card";
            }
            _context.EventCards.Remove(eventCard);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

    }
}
