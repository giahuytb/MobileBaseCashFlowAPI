using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class MobileBasedCashFlowGameContext : DbContext
    {
        public MobileBasedCashFlowGameContext()
        {
        }

        public MobileBasedCashFlowGameContext(DbContextOptions<MobileBasedCashFlowGameContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Friendship> Friendships { get; set; } = null!;
        public virtual DbSet<FriendshipStatus> FriendshipStatuses { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameMatch> GameMatches { get; set; } = null!;
        public virtual DbSet<GameReport> GameReports { get; set; } = null!;
        public virtual DbSet<Inventory> Inventories { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Leaderboard> Leaderboards { get; set; } = null!;
        public virtual DbSet<LoginHistory> LoginHistories { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.HasKey(e => new { e.RequesterId, e.AddresseeId })
                    .HasName("Friendship_PK");

                entity.ToTable("Friendship");

                entity.Property(e => e.RequesterId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("requester_id")
                    .IsFixedLength();

                entity.Property(e => e.AddresseeId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("addressee_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.HasOne(d => d.Addressee)
                    .WithMany(p => p.FriendshipAddressees)
                    .HasForeignKey(d => d.AddresseeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FriendshipToAddressee_FK");

                entity.HasOne(d => d.Requester)
                    .WithMany(p => p.FriendshipRequesters)
                    .HasForeignKey(d => d.RequesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FriendshipToRequester_FK");
            });

            modelBuilder.Entity<FriendshipStatus>(entity =>
            {
                entity.HasKey(e => new { e.RequesterId, e.AddresseeId, e.SpecifiedDateTime })
                    .HasName("FriendshipStatus_PK");

                entity.ToTable("FriendshipStatus");

                entity.Property(e => e.RequesterId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("requester_id")
                    .IsFixedLength();

                entity.Property(e => e.AddresseeId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("Addressee_id")
                    .IsFixedLength();

                entity.Property(e => e.SpecifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.SpecifierId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Specifier)
                    .WithMany(p => p.FriendshipStatuses)
                    .HasForeignKey(d => d.SpecifierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FriendshipStatusToSpecifier_FK");

                entity.HasOne(d => d.Friendship)
                    .WithMany(p => p.FriendshipStatuses)
                    .HasForeignKey(d => new { d.RequesterId, d.AddresseeId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FriendshipStatusToFriendship_FK");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("game_id")
                    .IsFixedLength();

                entity.Property(e => e.BackgroundImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("background_image_url");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.GameVersion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("game_version");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");
            });

            modelBuilder.Entity<GameMatch>(entity =>
            {
                entity.HasKey(e => e.MatchId)
                    .HasName("PK__Game_mat__9D7FCBA3B0B514F9");

                entity.ToTable("Game_match");

                entity.Property(e => e.MatchId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("match_id")
                    .IsFixedLength();

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("game_id")
                    .IsFixedLength();

                entity.Property(e => e.HostId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("host_id")
                    .IsFixedLength();

                entity.Property(e => e.LastHostId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("last_host_id")
                    .IsFixedLength();

                entity.Property(e => e.MaxNumberPlayer).HasColumnName("max_number_player");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");

                entity.Property(e => e.TotalRound).HasColumnName("total_round");

                entity.Property(e => e.WinnerId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("winner_id")
                    .IsFixedLength();

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameMatches)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Game_matc__game___5AEE82B9");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.GameMatchHosts)
                    .HasForeignKey(d => d.HostId)
                    .HasConstraintName("FK__Game_matc__host___59063A47");

                entity.HasOne(d => d.LastHost)
                    .WithMany(p => p.GameMatchLastHosts)
                    .HasForeignKey(d => d.LastHostId)
                    .HasConstraintName("FK__Game_matc__last___59FA5E80");

                entity.HasOne(d => d.Winner)
                    .WithMany(p => p.GameMatchWinners)
                    .HasForeignKey(d => d.WinnerId)
                    .HasConstraintName("FK__Game_matc__winne__5812160E");
            });

            modelBuilder.Entity<GameReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__Game_rep__779B7C586A210AB9");

                entity.ToTable("Game_report");

                entity.Property(e => e.ReportId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("report_id")
                    .IsFixedLength();

                entity.Property(e => e.ChildrenAmount).HasColumnName("children_amount");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.ExpensePerMonth).HasColumnName("expense_per_month");

                entity.Property(e => e.IncomePerMonth).HasColumnName("income_per_month");

                entity.Property(e => e.IsWin).HasColumnName("is_win");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.TotalMoney).HasColumnName("total_money");

                entity.Property(e => e.TotalStep).HasColumnName("total_step");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("user_id")
                    .IsFixedLength();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Game_repo__user___619B8048");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ItemId })
                    .HasName("pk_inventory_id");

                entity.ToTable("Inventory");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("user_id")
                    .IsFixedLength();

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("item_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__item___534D60F1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__user___52593CB8");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("item_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.IsInShop).HasColumnName("is_in_shop");

                entity.Property(e => e.ItemImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("item_image_url");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(50)
                    .HasColumnName("item_name");

                entity.Property(e => e.ItemPrice).HasColumnName("item_price");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("update_by")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Leaderboard>(entity =>
            {
                entity.ToTable("Leaderboard");

                entity.Property(e => e.LeaderBoardId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("leader_board_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("game_id")
                    .IsFixedLength();

                entity.Property(e => e.PlayerId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("player_id")
                    .IsFixedLength();

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.TimePeriod).HasColumnName("time_period");

                entity.Property(e => e.TimePeriodFrom)
                    .HasColumnType("date")
                    .HasColumnName("time_period_from");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Leaderboards)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Leaderboa__game___4316F928");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Leaderboards)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK__Leaderboa__playe__440B1D61");
            });

            modelBuilder.Entity<LoginHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Login_history");

                entity.Property(e => e.LoginDate)
                    .HasColumnType("datetime")
                    .HasColumnName("login_date");

                entity.Property(e => e.LoginId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("login_id")
                    .IsFixedLength();

                entity.Property(e => e.LogoutDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Logout_date");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("user_id")
                    .IsFixedLength();

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Login_his__user___5535A963");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MatchId })
                    .HasName("pk_participant_id");

                entity.ToTable("Participant");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("user_id")
                    .IsFixedLength();

                entity.Property(e => e.MatchId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("match_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__match__5EBF139D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__user___5DCAEF64");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__User_acc__B9BE370FD728EF0A");

                entity.ToTable("User_account");

                entity.HasIndex(e => e.NickName, "UQ__User_acc__08E8937A6758E239")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "UQ__User_acc__7C9273C466494BE6")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("user_id")
                    .IsFixedLength();

                entity.Property(e => e.AvatarImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("avatar_image_url");

                entity.Property(e => e.Coin).HasColumnName("coin");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.EmailConfirmToken)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("email_confirm_token");

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("game_id")
                    .IsFixedLength();

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .HasColumnName("gender");

                entity.Property(e => e.NickName)
                    .HasMaxLength(30)
                    .HasColumnName("nick_name");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("password_hash");

                entity.Property(e => e.PasswordResetToken)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("password_reset_token");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.ResetTokenExpire)
                    .HasColumnType("datetime")
                    .HasColumnName("reset_token_expire");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("role_id")
                    .IsFixedLength();

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UserName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.VerifyAt)
                    .HasColumnType("datetime")
                    .HasColumnName("verify_at");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__User_acco__game___3F466844");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User_acco__role___403A8C7D");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__User_rol__760965CCFFA1A878");

                entity.ToTable("User_role");

                entity.HasIndex(e => e.RoleName, "UQ__User_rol__783254B15AAB4E45")
                    .IsUnique();

                entity.Property(e => e.RoleId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("role_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(10)
                    .HasColumnName("role_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
