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
                                         j.JobCardId,
                                         j.JobCardName,
                                         j.JobCardImageUrl,
                                         j.ChildrenCost,
                                         j.CreateAt,
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
                        j.JobCardId,
                        j.JobCardName,
                        j.JobCardImageUrl,
                        j.ChildrenCost,
                        j.CreateAt,
                    })
                    .Where(b => b.JobCardId == id)
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
                    JobCardName = jobCard.JobName,
                    JobCardImageUrl = jobCard.JobImageUrl,
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
                    oldJobCard.JobCardName = jobCard.JobName;
                    oldJobCard.JobCardImageUrl = jobCard.JobImageUrl;
                    oldJobCard.ChildrenCost = jobCard.ChildrenCost;
                    oldJobCard.UpdateAt = DateTime.Now;
                    oldJobCard.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this job card";
        }

        public async Task<string> DeleteAsync(string jobCardId)
        {
            var jobCard = await _context.JobCards.FindAsync(jobCardId);
            if (jobCard == null)
            {
                return "Can not find this job card";
            }
            _context.JobCards.Remove(jobCard);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

    }
}
