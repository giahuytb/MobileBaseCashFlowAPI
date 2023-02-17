using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class JobCard
    {
        public JobCard()
        {
            FinacialReports = new HashSet<FinacialReport>();
            JobAccounts = new HashSet<JobAccount>();
        }

        public string JobCardId { get; set; } = null!;
        public string JobName { get; set; } = null!;
        public string JobImageUrl { get; set; } = null!;
        public double ChildrenCost { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }

        public virtual ICollection<FinacialReport> FinacialReports { get; set; }
        public virtual ICollection<JobAccount> JobAccounts { get; set; }
    }
}
