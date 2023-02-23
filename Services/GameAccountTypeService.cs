using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameAccountTypeService : IGameAccountTypeService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public GameAccountTypeService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var accountType = await (from accType in _context.GameAccountTypes
                                         select new
                                         {
                                             accountTypeId = accType.AccountTypeId,
                                             accountTypeName = accType.AccountTypeName,
                                             createAt = accType.CreateAt,
                                         }).ToListAsync();
                return accountType;
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
                var accountType = await _context.GameAccountTypes
                    .Select(accType => new
                    {
                        accountTypeId = accType.AccountTypeId,
                        accountTypeName = accType.AccountTypeName,
                        createAt = accType.CreateAt,
                    })
                    .Where(i => i.accountTypeId == id)
                    .FirstOrDefaultAsync();
                return accountType;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CreateAsync(string userId, GameAccountTypeRequest gameAccountType)
        {
            try
            {
                var accountType = new GameAccountType()
                {
                    AccountTypeId = Guid.NewGuid().ToString(),
                    AccountTypeName = gameAccountType.AccountTypeName,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                };

                _context.GameAccountTypes.Add(accountType);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string accountTypeId, string userId, GameAccountTypeRequest gameAccountType)
        {
            var oldAccountType = await _context.GameAccountTypes.FirstOrDefaultAsync(i => i.AccountTypeId == accountTypeId);
            if (oldAccountType != null)
            {
                try
                {
                    oldAccountType.AccountTypeName = gameAccountType.AccountTypeName;
                    oldAccountType.UpdateAt = DateTime.Now;
                    oldAccountType.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this game account type";
        }

        public async Task<string> DeleteAsync(string accountTypeId)
        {
            var accountType = await _context.GameAccountTypes.FindAsync(accountTypeId);
            if (accountType == null)
            {
                return "Can not find this game account type";
            }
            _context.GameAccountTypes.Remove(accountType);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }


    }
}
