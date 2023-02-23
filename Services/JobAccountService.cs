using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class JobAccountService : IJobAccountService
    {
        public const string SUCCESS = "success";

        private readonly MobileBasedCashFlowGameContext _context;

        public JobAccountService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var JobCard = await (from jobAcc in _context.JobAccounts
                                     join job in _context.JobCards on jobAcc.JobCardId equals job.JobCardId
                                     join account in _context.GameAccounts on jobAcc.GameAccountId equals account.GameAccountId
                                     select new
                                     {
                                         jobName = job.JobName,
                                         accountName = account.GameAccountName,
                                         value = jobAcc.Value,
                                     }).ToListAsync();
                return JobCard;
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
                var allJobCard = await (from jobAcc in _context.JobAccounts
                                        join job in _context.JobCards on jobAcc.JobCardId equals job.JobCardId
                                        join account in _context.GameAccounts on jobAcc.GameAccountId equals account.GameAccountId
                                        select new
                                        {
                                            jobCardId = jobAcc.JobCardId,
                                            accountId = jobAcc.GameAccountId,
                                            jobName = job.JobName,
                                            accountName = account.GameAccountName,
                                            value = jobAcc.Value,
                                        }).ToListAsync();

                if (searchBy.Equals("jobCard"))
                {
                    allJobCard = allJobCard.Where(i => i.jobCardId == id).ToList();
                }
                else if (searchBy.Equals("account"))
                {
                    allJobCard = allJobCard.Where(i => i.accountId == id).ToList();
                }
                else
                {
                    return "Your searchBy field must be financial or account";
                }
                return allJobCard;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string JobCardId, string GameAccountId, double value)
        {
            // check if this job card have this account
            var check = await _context.JobAccounts.FirstOrDefaultAsync(i => i.JobCardId == JobCardId && i.GameAccountId == GameAccountId);
            if (check != null)
            {
                return "Your job card already have this account";
            }
            try
            {
                var jobAcc = new JobAccount()
                {
                    JobCardId = JobCardId,
                    GameAccountId = GameAccountId,
                    Value = value,
                };
                _context.JobAccounts.Add(jobAcc);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

    }
}
