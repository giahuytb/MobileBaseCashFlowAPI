using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class GameReportService : IGameReportRepository
    {
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
                                }).AsNoTracking().ToListAsync();
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
                                }).AsNoTracking().ToListAsync();
            return report;
        }

        public async Task<string> CreateAsync(int userId, GameReportRequest request)
        {
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
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int reportId, GameReportRequest request)
        {

            var oldGameReport = await _context.GameReports.Where(g => g.ReportId == reportId).FirstOrDefaultAsync();
            if (oldGameReport != null)
            {
                oldGameReport.ChildrenAmount = request.ChildrenAmount;
                oldGameReport.TotalStep = request.TotalStep;
                oldGameReport.TotalMoney = request.TotalMoney;
                oldGameReport.IsWin = request.IsWin;
                oldGameReport.Score = request.Score;
                oldGameReport.IncomePerMonth = request.IncomePerMonth;
                oldGameReport.ExpensePerMonth = request.ExpensePerMonth;
                await _context.SaveChangesAsync();
                return Constant.Success;
            };
            return "Can not found this game report";
        }

        public async Task<string> DeleteAsync(int reportId)
        {
            var gameReport = await _context.GameReports.Where(g => g.ReportId == reportId).FirstOrDefaultAsync();
            if (gameReport != null)
            {
                _context.GameReports.Remove(gameReport);
                await _context.SaveChangesAsync();
            }
            return "Can not found this game report";
        }


    }
}
