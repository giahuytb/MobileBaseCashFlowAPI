using System;
using System.Collections.Generic;

namespace MobieBaseCashFlowAPI.Models
{
    public partial class GameAccountType
    {
        public GameAccountType()
        {
            GameAccounts = new HashSet<GameAccount>();
        }

        public string AccountTypeId { get; set; } = null!;
        public string AccountTypeName { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }

        public virtual ICollection<GameAccount> GameAccounts { get; set; }
    }
}
