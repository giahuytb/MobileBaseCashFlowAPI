using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class GameReportRepository : IGameReportRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public GameReportRepository(MobileBasedCashFlowGameContext context)
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

        public async Task<object?> MyReport(int userId)
        {
            var report = await (from gr in _context.GameReports
                                join user in _context.UserAccounts on gr.UserId equals user.UserId
                                join match in _context.GameMatches on gr.MatchId equals match.MatchId
                                where gr.UserId == userId
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
                                    user.NickName,
                                    match.MatchId,
                                    MatchTime = match.EndTime - match.StartTime,
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


    }
}
