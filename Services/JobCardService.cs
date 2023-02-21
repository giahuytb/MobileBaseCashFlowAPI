using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class JobCardService : IJobCardService
    {
        public const string SUCCESS = "success";
        public const string FAILED = "failed";
        public const string NOTFOUND = "not found";
        private MobileBasedCashFlowGameContext _context;

        public JobCardService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var jobCard = await (from j in _context.JobCards
                                     select new
                                     {
                                         jobCardId = j.JobCardId,
                                         jobCardName = j.JobName,
                                         jobImageUrl = j.JobImageUrl,
                                         childrenCost = j.ChildrenCost,
                                         createAt = j.CreateAt,
                                     }).ToListAsync();
                return jobCard;
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
                var jobCard = await _context.JobCards
                    .Select(j => new
                    {
                        jobCardId = j.JobCardId,
                        jobCardName = j.JobName,
                        jobImageUrl = j.JobImageUrl,
                        childrenCost = j.ChildrenCost,
                        createAt = j.CreateAt,
                    })
                    .Where(b => b.jobCardId == id)
                    .FirstOrDefaultAsync();
                return jobCard;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CreateAsync(string userId, JobCardRequest jobCard)
        {
            try
            {
                if (!ValidateInput.isNumber(jobCard.ChildrenCost.ToString()) || jobCard.ChildrenCost <= 0)
                {
                    return "Children cost must be mumber and bigger than 0";
                }
                var jobCard1 = new JobCard()
                {
                    JobCardId = Guid.NewGuid().ToString(),
                    JobName = jobCard.JobName,
                    JobImageUrl = jobCard.JobImageUrl,
                    ChildrenCost = jobCard.ChildrenCost,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                };

                _context.JobCards.Add(jobCard1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string jobCardId, string userId, JobCardRequest jobCard)
        {
            var oldJobCard = await _context.JobCards.FirstOrDefaultAsync(i => i.JobCardId == jobCardId);
            if (oldJobCard != null)
            {
                try
                {
                    if (!ValidateInput.isNumber(jobCard.ChildrenCost.ToString()) || jobCard.ChildrenCost <= 0)
                    {
                        return "Children cost must be mumber and bigger than 0";
                    }
                    oldJobCard.JobName = jobCard.JobName;
                    oldJobCard.JobImageUrl = jobCard.JobImageUrl;
                    oldJobCard.ChildrenCost = jobCard.ChildrenCost;
                    oldJobCard.UpdateAt = DateTime.Now;
                    oldJobCard.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    if (!JobCardExists(jobCardId))
                    {
                        return NOTFOUND;
                    }
                    return ex.ToString();
                }
            }
            return FAILED;
        }

        public async Task<string> DeleteAsync(string jobCardId)
        {
            var jobCard = await _context.JobCards.FindAsync(jobCardId);
            if (jobCard == null)
            {
                return NOTFOUND;
            }
            _context.JobCards.Remove(jobCard);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

        private bool JobCardExists(string id)
        {
            return _context.JobCards.Any(e => e.JobCardId == id);
        }

    }
}
