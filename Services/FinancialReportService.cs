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
                var financialReport = await (from finan in _context.FinancialReports
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
                var financialReport = await _context.FinancialReports
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
                return financialReport;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string userId, FinancialReportRequest financialReport)
        {
            if (!ValidateInput.isNumber(financialReport.ChildrenAmount.ToString()) || financialReport.ChildrenAmount < 0)
            {
                return "Children amount must be mumber, equal or bigger than 0";
            }
            else if (!ValidateInput.isNumber(financialReport.IncomePerMonth.ToString()) || financialReport.IncomePerMonth <= 0)
            {
                return "Income per month must be mumber and bigger than 0";
            }
            else if (!ValidateInput.isNumber(financialReport.ExpensePerMonth.ToString()) || financialReport.ExpensePerMonth <= 0)
            {
                return "Expense per month must be mumber and bigger than 0";
            }
            try
            {
                var finan = new FinancialReport()
                {
                    FinacialId = Guid.NewGuid().ToString(),
                    ChildrenAmount = financialReport.ChildrenAmount,
                    IncomePerMonth = financialReport.IncomePerMonth,
                    ExpensePerMonth = financialReport.ExpensePerMonth,
                    JobCardId = "",
                    UserId = userId,
                    CreateAt = DateTime.Now,
                };

                _context.FinancialReports.Add(finan);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(string fianacialId, string userId, FinancialReportRequest financialReport)
        {
            var oldFinancial = await _context.FinancialReports.FirstOrDefaultAsync(i => i.FinacialId == fianacialId);
            if (oldFinancial != null)
            {
                try
                {
                    if (!ValidateInput.isNumber(financialReport.ChildrenAmount.ToString()) || financialReport.ChildrenAmount < 0)
                    {
                        return "Children amount must be mumber, equal or bigger than 0";
                    }
                    else if (!ValidateInput.isNumber(financialReport.IncomePerMonth.ToString()) || financialReport.IncomePerMonth <= 0)
                    {
                        return "Income per month must be mumber and bigger than 0";
                    }
                    else if (!ValidateInput.isNumber(financialReport.ExpensePerMonth.ToString()) || financialReport.ExpensePerMonth <= 0)
                    {
                        return "Expense per month must be mumber and bigger than 0";
                    }
                    oldFinancial.ChildrenAmount = financialReport.ChildrenAmount;
                    oldFinancial.IncomePerMonth = financialReport.IncomePerMonth;
                    oldFinancial.ExpensePerMonth = financialReport.ExpensePerMonth;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    if (!FinancialReportExists(fianacialId))
                    {
                        return NOTFOUND;
                    }
                    return ex.ToString();
                }
            }
            return FAILED;
        }
        public async Task<string> DeleteAsync(string fianacialId)
        {
            var financialReport = await _context.FinancialReports.FindAsync(fianacialId);
            if (financialReport == null)
            {
                return NOTFOUND;
            }
            _context.FinancialReports.Remove(financialReport);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

        private bool FinancialReportExists(string id)
        {
            return _context.FinancialReports.Any(e => e.FinacialId == id);
        }
    }
}
