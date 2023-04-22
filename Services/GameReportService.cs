using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameReportService : GameReportRepository
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public GameReportService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            var report = await (from gr in _context.GameReports
                                join user in _context.UserAccounts on gr.UserId equals user.UserId
                                join match in _context.GameMatches on gr.MatchId equals match.MatchId
                                select new
                                {
                                    gr.ReportId,
                                    gr.ChildrenAmount,
                                    gr.TotalStep,
                                    gr.TotalMoney,
                                    gr.IsWin,
                                    gr.Score,
                                    gr.IncomePerMonth,
                                    gr.ExpensePerMonth,
                                    gr.CreateAt,
                                    user.NickName,
                                    match.MatchId,
                                }).ToListAsync();
            return report;
        }

        public async Task<object?> GetAsync(int reportId)
        {
            var report = await (from gr in _context.GameReports
                                join user in _context.UserAccounts on gr.UserId equals user.UserId
                                join match in _context.GameMatches on gr.MatchId equals match.MatchId
                                where gr.ReportId == reportId
                                select new
                                {
                                    gr.ReportId,
                                    gr.ChildrenAmount,
                                    gr.TotalStep,
                                    gr.TotalMoney,
                                    gr.IsWin,
                                    gr.Score,
                                    gr.IncomePerMonth,
                                    gr.ExpensePerMonth,
                                    gr.CreateAt,
                                    user.NickName,
                                    match.MatchId,
                                }).ToListAsync();
            return report;
        }

        public async Task<string> CreateAsync(int userId, GameReportRequest request)
        {
            try
            {

                if (!ValidateInput.isNumber(request.ChildrenAmount.ToString()) || request.ChildrenAmount < 0)
                {
                    return "Children amount must be mumber and bigger than or equal 0";
                }
                if (!ValidateInput.isNumber(request.TotalStep.ToString()) || request.TotalStep <= 0)
                {
                    return "Total step must be mumber and bigger than 0";
                }
                if (!ValidateInput.isNumber(request.TotalMoney.ToString()) || request.TotalMoney <= 0)
                {
                    return "Total money must be mumber and bigger than 0";
                }
                if (!ValidateInput.isNumber(request.Score.ToString()) || request.Score <= 0)
                {
                    return "Score must be mumber and bigger than 0";
                }
                if (!ValidateInput.isNumber(request.IncomePerMonth.ToString()) || request.IncomePerMonth <= 0)
                {
                    return "Income per month must be mumber and bigger than 0";
                }
                if (!ValidateInput.isNumber(request.ExpensePerMonth.ToString()) || request.ExpensePerMonth <= 0)
                {
                    return "Expense per month must be mumber and bigger than 0";
                }


                var gameReport = new GameReport()
                {
                    ChildrenAmount = request.ChildrenAmount,
                    TotalStep = request.TotalStep,
                    TotalMoney = request.TotalMoney,
                    IsWin = request.IsWin,
                    Score = request.Score,
                    IncomePerMonth = request.IncomePerMonth,
                    ExpensePerMonth = request.ExpensePerMonth,
                    CreateAt = DateTime.Now,
                    UserId = userId,
                };

                _context.GameReports.Add(gameReport);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UpdateAsync(int reportId, GameReportRequest request)
        {
            try
            {
                var oldGameReport = await _context.GameReports.Where(g => g.ReportId == reportId).FirstOrDefaultAsync();
                if (oldGameReport != null)
                {
                    if (!ValidateInput.isNumber(request.ChildrenAmount.ToString()) || request.ChildrenAmount < 0)
                    {
                        return "Children amount must be mumber and bigger than or equal 0";
                    }
                    if (!ValidateInput.isNumber(request.TotalStep.ToString()) || request.TotalStep <= 0)
                    {
                        return "Total step must be mumber and bigger than 0";
                    }
                    if (!ValidateInput.isNumber(request.TotalMoney.ToString()) || request.TotalMoney <= 0)
                    {
                        return "Total money must be mumber and bigger than 0";
                    }
                    if (!ValidateInput.isNumber(request.Score.ToString()) || request.Score <= 0)
                    {
                        return "Score must be mumber and bigger than 0";
                    }
                    if (!ValidateInput.isNumber(request.IncomePerMonth.ToString()) || request.IncomePerMonth <= 0)
                    {
                        return "Income per month must be mumber and bigger than 0";
                    }
                    if (!ValidateInput.isNumber(request.ExpensePerMonth.ToString()) || request.ExpensePerMonth <= 0)
                    {
                        return "Expense per month must be mumber and bigger than 0";
                    }

                    oldGameReport.ChildrenAmount = request.ChildrenAmount;
                    oldGameReport.TotalStep = request.TotalStep;
                    oldGameReport.TotalMoney = request.TotalMoney;
                    oldGameReport.IsWin = request.IsWin;
                    oldGameReport.Score = request.Score;
                    oldGameReport.IncomePerMonth = request.IncomePerMonth;
                    oldGameReport.ExpensePerMonth = request.ExpensePerMonth;
                    await _context.SaveChangesAsync();
                    return SUCCESS;
                };
                return "Can not found this game report";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> DeleteAsync(int reportId)
        {
            var gameReport = await _context.GameReports.Where(g => g.ReportId == reportId).FirstOrDefaultAsync();
            if (gameReport != null)
            {
                _context.GameReports.Remove(gameReport);
            }
            return "Can not found this game report";
        }


    }
}
