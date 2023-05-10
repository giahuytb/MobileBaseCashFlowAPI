using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            GameMatchHosts = new HashSet<GameMatch>();
            GameMatchLastHosts = new HashSet<GameMatch>();
            GameMatchWinners = new HashSet<GameMatch>();
            GameReports = new HashSet<GameReport>();
            Participants = new HashSet<Participant>();
            UserAssets = new HashSet<UserAsset>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? NickName { get; set; }
        public string Email { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? Phone { get; set; }
        public double? Coin { get; set; }
        public string? Gender { get; set; }
        public bool Status { get; set; }
        public string? EmailConfirmToken { get; set; }
        public DateTime? VerifyAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpire { get; set; }
        public string? Address { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? GameServerId { get; set; }
        public int? RoleId { get; set; }
        public double? Point { get; set; }
        public string? LastJobSelected { get; set; }

        public virtual GameServer? GameServer { get; set; }
        public virtual UserRole? Role { get; set; }
        public virtual ICollection<GameMatch> GameMatchHosts { get; set; }
        public virtual ICollection<GameMatch> GameMatchLastHosts { get; set; }
        public virtual ICollection<GameMatch> GameMatchWinners { get; set; }
        public virtual ICollection<GameReport> GameReports { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<UserAsset> UserAssets { get; set; }
    }
}
