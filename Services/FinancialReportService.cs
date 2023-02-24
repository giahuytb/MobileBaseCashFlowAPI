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
                                                 finan.FinacialId,
                                                 finan.ChildrenAmount,
                                                 finan.IncomePerMonth,
                                                 finan.ExpensePerMonth,
                                                 job.JobCardName,
                                                 finan.CreateAt,
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
                        m.finan.FinacialId,
                        m.finan.ChildrenAmount,
                        m.finan.IncomePerMonth,
                        m.finan.ExpensePerMonth,
                        m.job.JobCardName,
                        m.finan.CreateAt,
                    })
                    .Where(i => i.FinacialId == id)
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
            var oldFinancial = await _context.FinancialReports.Where(i => i.FinacialId == fianacialId).FirstOrDefaultAsync();
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
                    return ex.ToString();
                }
            }
            return "Can not find this financial report";
        }
        public async Task<string> DeleteAsync(string fianacialId)
        {
            var financialReport = await _context.FinancialReports.FindAsync(fianacialId);
            if (financialReport == null)
            {
                return "Can not find this financial report";
            }
            _context.FinancialReports.Remove(financialReport);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }


    }
}
