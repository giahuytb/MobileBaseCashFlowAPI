using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class GameAccount
    {
        public GameAccount()
        {
            FinacialAccounts = new HashSet<FinacialAccount>();
            JobAccounts = new HashSet<JobAccount>();
        }

        public string GameAccountId { get; set; } = null!;
        public string GameAccountName { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }
        public string? AccountTypeId { get; set; }

        public virtual GameAccountType? AccountType { get; set; }
        public virtual ICollection<FinacialAccount> FinacialAccounts { get; set; }
        public virtual ICollection<JobAccount> JobAccounts { get; set; }
    }
}
