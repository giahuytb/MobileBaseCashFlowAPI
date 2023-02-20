using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class FinancialReport
    {
        public FinancialReport()
        {
            FinancialAccounts = new HashSet<FinancialAccount>();
        }

        public string FinacialId { get; set; } = null!;
        public int ChildrenAmount { get; set; }
        public double IncomePerMonth { get; set; }
        public double ExpensePerMonth { get; set; }
        public DateTime CreateAt { get; set; }
        public string? UserId { get; set; }
        public string? JobCardId { get; set; }

        public virtual JobCard? JobCard { get; set; }
        public virtual UserAccount? User { get; set; }
        public virtual ICollection<FinancialAccount> FinancialAccounts { get; set; }
    }
}
