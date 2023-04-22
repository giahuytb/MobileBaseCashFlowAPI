using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }

        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
