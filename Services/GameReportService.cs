using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameReportService : IGameReportService
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
                                }).ToListAsync();
            return report;
        }

        public async Task<object?> GetAsync(string id)
        {
            var report = await (from gr in _context.GameReports
                                join user in _context.UserAccounts on gr.UserId equals user.UserId
                                where gr.ReportId == id
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
                                }).ToListAsync();
            return report;
        }

        public async Task<string> CreateAsync(string userId, GameReportRequest request)
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
                    ReportId = Guid.NewGuid().ToString(),
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

        public Task<string> UpdateAsync(string reportId, GameReportRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteAsync(string reportId)
        {
            throw new NotImplementedException();
        }



    }
}
