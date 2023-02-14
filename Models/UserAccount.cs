using System;
using System.Collections.Generic;

namespace MobieBasedCashFlowAPI.Models
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            FinacialReports = new HashSet<FinacialReport>();
            FriendshipAddressees = new HashSet<Friendship>();
            FriendshipRequesters = new HashSet<Friendship>();
            FriendshipStatuses = new HashSet<FriendshipStatus>();
            GameMatchHosts = new HashSet<GameMatch>();
            GameMatchLastHosts = new HashSet<GameMatch>();
            GameMatchWiners = new HashSet<GameMatch>();
            Inventories = new HashSet<Inventory>();
            Leaderboards = new HashSet<Leaderboard>();
            Matches = new HashSet<GameMatch>();
        }

        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string AvatarImageUrl { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public bool Status { get; set; }
        public string? EmailConfirmToken { get; set; }
        public DateTime? VerifyAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpire { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? GameId { get; set; }
        public string? RoleId { get; set; }

        public virtual Game? Game { get; set; }
        public virtual UserRole? Role { get; set; }
        public virtual ICollection<FinacialReport> FinacialReports { get; set; }
        public virtual ICollection<Friendship> FriendshipAddressees { get; set; }
        public virtual ICollection<Friendship> FriendshipRequesters { get; set; }
        public virtual ICollection<FriendshipStatus> FriendshipStatuses { get; set; }
        public virtual ICollection<GameMatch> GameMatchHosts { get; set; }
        public virtual ICollection<GameMatch> GameMatchLastHosts { get; set; }
        public virtual ICollection<GameMatch> GameMatchWiners { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Leaderboard> Leaderboards { get; set; }

        public virtual ICollection<GameMatch> Matches { get; set; }
    }
}
