using Microsoft.EntityFrameworkCore;
using System.Collections;

using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;

namespace MobileBasedCashFlowAPI.Services
{
    public class DreamService : IDreamService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public DreamService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var board = await (from d in _context.Dreams
                                   select new
                                   {
                                       dreamId = d.DreamId,
                                       dreamName = d.DreamName,
                                       description = d.Description,
                                       cost = d.Cost,
                                       dreamImageUrl = d.DreamImageUrl,
                                       createAt = d.CreateAt,
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
                var board = await _context.Dreams
                    .Select(b => new
                    {
                        dreamId = b.DreamId,
                        dreamName = b.DreamName,
                        description = b.Description,
                        cost = b.Cost,
                        dreamImageUrl = b.DreamImageUrl,
                        createAt = b.CreateAt,
                    })
                    .Where(d => d.dreamId == id)
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

        public async Task<string> CreateAsync(string userId, DreamRequest dream)
        {
            try
            {
                var checkName = await _context.Dreams
                                .Where(d => d.DreamName == dream.DreamName)
                                .Select(d => new { dreamName = d.DreamName })
                                .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This dream name is existed";
                }
                if (!ValidateInput.isNumber(dream.Cost.ToString()) || dream.Cost <= 0)
                {
                    return "Cost must be mumber and bigger than 0";
                }

                var board1 = new Dream()
                {
                    DreamId = Guid.NewGuid().ToString(),
                    DreamName = dream.DreamName,
                    Description = dream.Description,
                    Cost = dream.Cost,
                    DreamImageUrl = dream.DreamImageUrl,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                };
                _context.Dreams.Add(board1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string dreamId, string userId, DreamRequest dream)
        {
            var oldDream = await _context.Dreams.Where(d => d.DreamId == dreamId).FirstOrDefaultAsync();
            if (oldDream != null)
            {
                try
                {
                    var checkName = await _context.Dreams
                                .Where(d => d.DreamName == dream.DreamName)
                                .Select(d => new { dreamName = d.DreamName })
                                .FirstOrDefaultAsync();
                    if (checkName != null)
                    {
                        return "This dream name is existed";
                    }
                    if (!ValidateInput.isNumber(dream.Cost.ToString()) || dream.Cost <= 0)
                    {
                        return "Cost must be mumber and bigger than 0";
                    }
                    oldDream.DreamName = dream.DreamName;
                    oldDream.Description = dream.Description;
                    oldDream.DreamImageUrl = dream.DreamImageUrl;
                    oldDream.Cost = dream.Cost;
                    oldDream.UpdateAt = DateTime.Now;
                    oldDream.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this dream";
        }

        public async Task<string> DeleteAsync(string dreamId)
        {
            var dream = await _context.Dreams.FindAsync(dreamId);
            if (dream != null)
            {
                _context.Dreams.Remove(dream);
                await _context.SaveChangesAsync();

                return SUCCESS;
            }
            return "Can not find this dream";
        }

    }
}
