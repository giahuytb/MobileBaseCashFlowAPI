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
        public virtual DbSet<GameMod> GameMods { get; set; } = null!;
        public virtual DbSet<GameReport> GameReports { get; set; } = null!;
        public virtual DbSet<GameRoom> GameRooms { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<PointOfInteraction> PointOfInteractions { get; set; } = null!;
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
                    .HasColumnName("image_url");

                entity.Property(e => e.IsInShop).HasColumnName("is_in_shop");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.AssetTypeNavigation)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.AssetType)
                    .HasConstraintName("FK__Asset__asset_typ__59063A47");
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

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.HasIndex(e => e.GameVersion, "UQ__Game__3BAE19AC7863A55A")
                    .IsUnique();

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.GameVersion)
                    .HasMaxLength(20)
                    .HasColumnName("game_version");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            modelBuilder.Entity<GameMatch>(entity =>
            {
                entity.HasKey(e => e.MatchId)
                    .HasName("PK__Game_mat__9D7FCBA3F2FA1209");

                entity.ToTable("Game_match");

                entity.Property(e => e.MatchId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("match_id")
                    .IsFixedLength();

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.GameRoomId).HasColumnName("game_room_id");

                entity.Property(e => e.HostId).HasColumnName("host_id");

                entity.Property(e => e.LastHostId).HasColumnName("last_host_id");

                entity.Property(e => e.MaxNumberPlayer).HasColumnName("max_number_player");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");

                entity.Property(e => e.TotalRound).HasColumnName("total_round");

                entity.Property(e => e.WinnerId).HasColumnName("winner_id");

                entity.HasOne(d => d.GameRoom)
                    .WithMany(p => p.GameMatches)
                    .HasForeignKey(d => d.GameRoomId)
                    .HasConstraintName("FK__Game_matc__game___628FA481");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.GameMatchHosts)
                    .HasForeignKey(d => d.HostId)
                    .HasConstraintName("FK__Game_matc__host___60A75C0F");

                entity.HasOne(d => d.LastHost)
                    .WithMany(p => p.GameMatchLastHosts)
                    .HasForeignKey(d => d.LastHostId)
                    .HasConstraintName("FK__Game_matc__last___619B8048");

                entity.HasOne(d => d.Winner)
                    .WithMany(p => p.GameMatchWinners)
                    .HasForeignKey(d => d.WinnerId)
                    .HasConstraintName("FK__Game_matc__winne__5FB337D6");
            });

            modelBuilder.Entity<GameMod>(entity =>
            {
                entity.ToTable("Game_mod");

                entity.Property(e => e.GameModId).HasColumnName("game_mod_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.GameRoomId).HasColumnName("game_room_id");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

                entity.Property(e => e.ModName)
                    .HasMaxLength(20)
                    .HasColumnName("mod_name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.GameRoom)
                    .WithMany(p => p.GameMods)
                    .HasForeignKey(d => d.GameRoomId)
                    .HasConstraintName("FK__Game_mod__game_r__4E88ABD4");
            });

            modelBuilder.Entity<GameReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__Game_rep__779B7C58F9B1FAE0");

                entity.ToTable("Game_report");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.ChildrenAmount).HasColumnName("children_amount");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.ExpensePerMonth).HasColumnName("expense_per_month");

                entity.Property(e => e.IncomePerMonth).HasColumnName("income_per_month");

                entity.Property(e => e.IsWin).HasColumnName("is_win");

                entity.Property(e => e.MatchId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("match_id")
                    .IsFixedLength();

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.TotalMoney).HasColumnName("total_money");

                entity.Property(e => e.TotalStep).HasColumnName("total_step");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.MatchId)
                    .HasConstraintName("FK__Game_repo__match__693CA210");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Game_repo__user___6A30C649");
            });

            modelBuilder.Entity<GameRoom>(entity =>
            {
                entity.ToTable("Game_room");

                entity.Property(e => e.GameRoomId).HasColumnName("game_room_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.RoomName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("room_name");

                entity.Property(e => e.RoomNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("room_number");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameRooms)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Game_room__game___4BAC3F29");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MatchId })
                    .HasName("pk_participant_id");

                entity.ToTable("Participant");

                entity.Property(e => e.UserId).HasColumnName("user_id");

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
                    .HasConstraintName("FK__Participa__match__66603565");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__user___656C112C");
            });

            modelBuilder.Entity<PointOfInteraction>(entity =>
            {
                entity.HasKey(e => e.PoiId)
                    .HasName("PK__Point_of__6176E7AC865CFB86");

                entity.ToTable("Point_of_interaction");

                entity.HasIndex(e => e.PoiName, "UQ__Point_of__0564CB7DFAC488CD")
                    .IsUnique();

                entity.Property(e => e.PoiId).HasColumnName("poi_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.PoiDescription)
                    .HasMaxLength(20)
                    .HasColumnName("poi_description");

                entity.Property(e => e.PoiName)
                    .HasMaxLength(20)
                    .HasColumnName("poi_name");

                entity.Property(e => e.PoiVideoUrl)
                    .HasMaxLength(200)
                    .HasColumnName("poi_video_url");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.PointOfInteractions)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Point_of___game___48CFD27E");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__User_acc__B9BE370F721FB3CC");

                entity.ToTable("User_account");

                entity.HasIndex(e => e.UserName, "UQ__User_acc__7C9273C4755C2478")
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

                entity.Property(e => e.GameId).HasColumnName("game_id");

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

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__User_acco__game___534D60F1");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User_acco__role___5441852A");
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
                    .HasConstraintName("FK__User_asse__asset__5CD6CB2B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAssets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_asse__user___5BE2A6F2");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__User_rol__760965CC9B826B3D");

                entity.ToTable("User_role");

                entity.HasIndex(e => e.RoleName, "UQ__User_rol__783254B15121F2D5")
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
