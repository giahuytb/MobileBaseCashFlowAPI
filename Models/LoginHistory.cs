using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class LoginHistory
    {
        public string LoginId { get; set; } = null!;
        public DateTime? LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public string? UserId { get; set; }

        public virtual UserAccount? User { get; set; }
    }
}
