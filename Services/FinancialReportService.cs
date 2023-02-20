using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;
using System.Linq;

namespace MobileBasedCashFlowAPI.Services
{
    public class FinancialReportService : IFinancialReportService
    {
        public const string SUCCESS = "success";
        public const string FAILED = "failed";
        public const string NOTFOUND = "not found";
        private readonly MobileBasedCashFlowGameContext _context;

        public FinancialReportService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var financialReport = await (from finan in _context.FinacialReports
                                             join job in _context.JobCards
                                             on finan.JobCardId equals job.JobCardId
                                             select new
                                             {
                                                 financialReportId = finan.FinacialId,
                                                 childrenAmount = finan.ChildrenAmount,
                                                 incomePerMonth = finan.IncomePerMonth,
                                                 expensePerMonth = finan.ExpensePerMonth,
                                                 jobCard = job.JobName,
                                                 createAt = finan.CreateAt,
                                             }).ToListAsync();
                return financialReport;
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
                var financialReport = await _context.FinacialReports
                    .Join(_context.JobCards, finan => finan.JobCardId, job => job.JobCardId, (finan, job) => new { finan, job })
                    .Select(m => new
                    {
                        financialReportId = m.finan.FinacialId,
                        childrenAmount = m.finan.ChildrenAmount,
                        incomePerMonth = m.finan.IncomePerMonth,
                        expensePerMonth = m.finan.ExpensePerMonth,
                        jobCard = m.job.JobName,
                        createAt = m.finan.CreateAt,
                    })
                    .Where(i => i.financialReportId == id)
                    .FirstOrDefaultAsync();
                return financialReport; ;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CreateAsync(string userId, FinancialReportRequest financialReport)
        {

            try
            {
                var finan = new FinacialReport()
                {
                    FinacialId = Guid.NewGuid().ToString(),
                    ChildrenAmount = financialReport.ChildrenAmount,
                    IncomePerMonth = financialReport.IncomePerMonth,
                    ExpensePerMonth = financialReport.ExpensePerMonth,
                    JobCardId = "",
                    UserId = userId,
                    CreateAt = DateTime.Now,
                };

                _context.FinacialReports.Add(finan);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public Task<string> UpdateAsync(string fianacialId, string userId, FinancialReportRequest financialReport)
        {
            throw new NotImplementedException();
        }
        public Task<string> DeleteAsync(string fianacialId)
        {
            throw new NotImplementedException();
        }

    }
}
