using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class GameReport
    {
        public int ReportId { get; set; }
        public int ChildrenAmount { get; set; }
        public int TotalStep { get; set; }
        public double TotalMoney { get; set; }
        public bool IsWin { get; set; }
        public double Score { get; set; }
        public double IncomePerMonth { get; set; }
        public double ExpensePerMonth { get; set; }
        public DateTime CreateAt { get; set; }
        public string? MatchId { get; set; }
        public int? UserId { get; set; }

        public virtual GameMatch? Match { get; set; }
        public virtual UserAccount? User { get; set; }
    }
}
