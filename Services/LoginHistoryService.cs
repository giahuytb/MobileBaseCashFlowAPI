using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class LoginHistoryService : ILoginHistoryService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public LoginHistoryService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var result = await (from log in _context.LoginHistories
                                    join user in _context.UserAccounts on log.UserId equals user.UserId
                                    select new
                                    {
                                        user.UserName,
                                        user.NickName,
                                        log.LoginDate,
                                        log.LogoutDate,
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<IEnumerable> GetAsync(string userId)
        {
            try
            {
                var result = await (from log in _context.LoginHistories
                                    join user in _context.UserAccounts on log.UserId equals user.UserId
                                    where log.UserId == userId
                                    select new
                                    {
                                        user.UserName,
                                        user.NickName,
                                        log.LoginDate,
                                        log.LogoutDate,
                                    }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> WriteLog(string userId)
        {
            try
            {
                var loginHistory = new LoginHistory
                {
                    LoginId = Guid.NewGuid().ToString(),
                    LoginDate = DateTime.Now,
                    UserId = userId,
                };
                await _context.LoginHistories.AddAsync(loginHistory);
                await _context.SaveChangesAsync();
                return loginHistory.LoginId;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateLog(string loginId)
        {
            try
            {
                var loginHistory = await _context.LoginHistories.Where(i => i.LoginId == loginId).FirstOrDefaultAsync();
                if (loginHistory == null)
                {
                    return "Can not found this login history";
                }
                loginHistory.LogoutDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
