using System;
using System.Collections.Generic;

namespace MobieBasedCashFlowAPI.Models
{
    public partial class LoginHistory
    {
        public string? LoginId { get; set; }
        public DateTime? LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public string? UserId { get; set; }

        public virtual UserAccount? User { get; set; }
    }
}
