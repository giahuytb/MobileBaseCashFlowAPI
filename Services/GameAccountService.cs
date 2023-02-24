using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameAccountService : IGameAccountService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public GameAccountService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var gameAccount = await (from acc in _context.GameAccounts
                                         join accType in _context.GameAccountTypes
                                         on acc.AccountTypeId equals accType.AccountTypeId
                                         select new
                                         {
                                             acc.GameAccountId,
                                             acc.GameAccountName,
                                             accType.AccountTypeName,
                                             acc.CreateAt,
                                         }).ToListAsync();
                return gameAccount;
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
                var gameAccount = await (from acc in _context.GameAccounts
                                         join accType in _context.GameAccountTypes
                                         on acc.AccountTypeId equals accType.AccountTypeId
                                         select new
                                         {
                                             acc.GameAccountId,
                                             acc.GameAccountName,
                                             accType.AccountTypeName,
                                             acc.CreateAt,
                                         })
                                         .Where(a => a.GameAccountId == id)
                                         .FirstOrDefaultAsync();
                return gameAccount;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CreateAsync(string userId, GameAccountRequest gameAccount)
        {
            try
            {
                var acc = new GameAccount()
                {
                    GameAccountId = Guid.NewGuid().ToString(),
                    GameAccountName = gameAccount.GameAccountName,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                    AccountTypeId = gameAccount.AccountTypeId,
                };

                _context.GameAccounts.Add(acc);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string gameAccountId, string userId, GameAccountRequest gameAccount)
        {
            var oldAccount = await _context.GameAccounts.FirstOrDefaultAsync(i => i.GameAccountId == gameAccountId);
            if (oldAccount != null)
            {
                try
                {
                    oldAccount.GameAccountName = gameAccount.GameAccountName;
                    oldAccount.AccountTypeId = gameAccount.AccountTypeId;
                    oldAccount.UpdateAt = DateTime.Now;
                    oldAccount.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this game account";
        }
        public async Task<string> DeleteAsync(string gameAccountId)
        {
            var acc = await _context.GameAccounts.FindAsync(gameAccountId);
            if (acc == null)
            {
                return "Can not find this game account";
            }
            _context.GameAccounts.Remove(acc);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

    }
}
