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

        public virtual DbSet<Asset> Assets { get; set; } = null!;
        public virtual DbSet<AssetType> AssetTypes { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameMatch> GameMatches { get; set; } = null!;
        public virtual DbSet<GameMode> GameModes { get; set; } = null!;
        public virtual DbSet<GameReport> GameReports { get; set; } = null!;
        public virtual DbSet<GameServer> GameServers { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public virtual DbSet<UserAsset> UserAssets { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("Asset");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.AssetName)
                    .HasMaxLength(50)
                    .HasColumnName("asset_name");

                entity.Property(e => e.AssetPrice).HasColumnName("asset_price");

                entity.Property(e => e.AssetType).HasColumnName("asset_type");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

                entity.Property(e => e.IsInShop).HasColumnName("is_in_shop");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.AssetTypeNavigation)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.AssetType)
                    .HasConstraintName("FK__Asset__asset_typ__49C3F6B7");
            });

            modelBuilder.Entity<AssetType>(entity =>
            {
                entity.ToTable("Asset_type");

                entity.Property(e => e.AssetTypeId).HasColumnName("asset_type_id");

                entity.Property(e => e.AssetTypeName)
                    .HasMaxLength(50)
                    .HasColumnName("asset_type_name");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.GameServerId).HasColumnName("game_server_id");

                entity.Property(e => e.RoomName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("room_name");

                entity.Property(e => e.RoomNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("room_number");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.GameServer)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.GameServerId)
                    .HasConstraintName("FK__Game__game_serve__3C69FB99");
            });

            modelBuilder.Entity<GameMatch>(entity =>
            {
                entity.HasKey(e => e.MatchId)
                    .HasName("PK__Game_mat__9D7FCBA3D556EE4B");

                entity.ToTable("Game_match");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.HostId).HasColumnName("host_id");

                entity.Property(e => e.LastHostId).HasColumnName("last_host_id");

                entity.Property(e => e.MaxNumberPlayer).HasColumnName("max_number_player");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");

                entity.Property(e => e.TotalRound).HasColumnName("total_round");

                entity.Property(e => e.WinnerId).HasColumnName("winner_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameMatches)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Game_matc__game___534D60F1");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.GameMatchHosts)
                    .HasForeignKey(d => d.HostId)
                    .HasConstraintName("FK__Game_matc__host___5165187F");

                entity.HasOne(d => d.LastHost)
                    .WithMany(p => p.GameMatchLastHosts)
                    .HasForeignKey(d => d.LastHostId)
                    .HasConstraintName("FK__Game_matc__last___52593CB8");

                entity.HasOne(d => d.Winner)
                    .WithMany(p => p.GameMatchWinners)
                    .HasForeignKey(d => d.WinnerId)
                    .HasConstraintName("FK__Game_matc__winne__5070F446");
            });

            modelBuilder.Entity<GameMode>(entity =>
            {
                entity.ToTable("Game_mode");

                entity.Property(e => e.GameModeId).HasColumnName("game_mode_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

                entity.Property(e => e.ModeName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("mode_name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameModes)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Game_mode__game___3F466844");
            });

            modelBuilder.Entity<GameReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__Game_rep__779B7C58CE7F1FB9");

                entity.ToTable("Game_report");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.ChildrenAmount).HasColumnName("children_amount");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.ExpensePerMonth).HasColumnName("expense_per_month");

                entity.Property(e => e.IncomePerMonth).HasColumnName("income_per_month");

                entity.Property(e => e.IsWin).HasColumnName("is_win");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.TotalMoney).HasColumnName("total_money");

                entity.Property(e => e.TotalStep).HasColumnName("total_step");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.MatchId)
                    .HasConstraintName("FK__Game_repo__match__59FA5E80");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Game_repo__user___5AEE82B9");
            });

            modelBuilder.Entity<GameServer>(entity =>
            {
                entity.ToTable("Game_server");

                entity.Property(e => e.GameServerId).HasColumnName("game_server_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.GameVersion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("game_version");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MatchId })
                    .HasName("pk_participant_id");

                entity.ToTable("Participant");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__match__571DF1D5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__user___5629CD9C");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__User_acc__B9BE370FD2834C3D");

                entity.ToTable("User_account");

                entity.HasIndex(e => e.UserName, "UQ__User_acc__7C9273C4C0C6E9FD")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(300)
                    .HasColumnName("address");

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

                entity.Property(e => e.GameServerId).HasColumnName("game_server_id");

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .HasColumnName("gender");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

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

                entity.Property(e => e.RoleId).HasColumnName("role_id");

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

                entity.HasOne(d => d.GameServer)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.GameServerId)
                    .HasConstraintName("FK__User_acco__game___440B1D61");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User_acco__role___44FF419A");
            });

            modelBuilder.Entity<UserAsset>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.AssetId })
                    .HasName("pk_userAsset_id");

                entity.ToTable("User_asset");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.LastUsed)
                    .HasColumnType("datetime")
                    .HasColumnName("last_used");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.UserAssets)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_asse__asset__4D94879B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAssets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_asse__user___4CA06362");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__User_rol__760965CCC05BBEFA");

                entity.ToTable("User_role");

                entity.HasIndex(e => e.RoleName, "UQ__User_rol__783254B1BA004980")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(10)
                    .HasColumnName("role_name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
