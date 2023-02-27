using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class GameReport
    {
        public string ReportId { get; set; } = null!;
        public int ChildrenAmount { get; set; }
        public int TotalStep { get; set; }
        public double TotalMoney { get; set; }
        public bool IsWin { get; set; }
        public double Score { get; set; }
        public double IncomePerMonth { get; set; }
        public double ExpensePerMonth { get; set; }
        public DateTime CreateAt { get; set; }
        public string? UserId { get; set; }

        public virtual UserAccount? User { get; set; }
    }
}
