using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class FinancialAccountService : IFinancialAccountService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public FinancialAccountService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var finanAccount = await (from finAcc in _context.FinancialAccounts
                                          join account in _context.GameAccounts on finAcc.GameAccountId equals account.GameAccountId
                                          select new
                                          {
                                              finAcc.FinacialId,
                                              account.GameAccountName,
                                              finAcc.Value,
                                          }).ToListAsync();
                return finanAccount;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object?> GetAsync(string searchBy, string id)
        {
            try
            {

                var allFinAcc = await (from finAcc in _context.FinancialAccounts
                                       join account in _context.GameAccounts on finAcc.GameAccountId equals account.GameAccountId
                                       select new
                                       {
                                           finAcc.FinacialId,
                                           account.GameAccountId,
                                           account.GameAccountName,
                                           finAcc.Value,
                                       }).ToListAsync();

                if (searchBy.Equals("financial"))
                {
                    allFinAcc = allFinAcc.Where(i => i.FinacialId == id).ToList();
                }
                else if (searchBy.Equals("account"))
                {
                    allFinAcc = allFinAcc.Where(i => i.GameAccountId == id).ToList();
                }
                else
                {
                    return "Your searchBy field must be financial or account";
                }
                return allFinAcc;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string finacialId, string gameAccountId, double value)
        {
            // check if this financial have this account
            var check = await _context.FinancialAccounts
                .Where(i => i.FinacialId == finacialId && i.GameAccountId == gameAccountId)
                .FirstOrDefaultAsync();
            if (check != null)
            {
                return "Your financial report already have this account";
            }
            try
            {
                var finAcc = new FinancialAccount()
                {
                    FinacialId = finacialId,
                    GameAccountId = gameAccountId,
                    Value = value,
                };
                _context.FinancialAccounts.Add(finAcc);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string finacialId, string gameAccountId, double value)
        {
            var oldFinanAcc = await _context.FinancialAccounts
                .Where(d => d.FinacialId == finacialId && d.GameAccountId == gameAccountId)
                .FirstOrDefaultAsync();

            if (oldFinanAcc != null)
            {
                try
                {
                    if (!ValidateInput.isNumber(value.ToString()) || value <= 0)
                    {
                        return "Value must be mumber and bigger than 0";
                    }
                    oldFinanAcc.Value = value;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this financial account"; ;
        }


    }
}
