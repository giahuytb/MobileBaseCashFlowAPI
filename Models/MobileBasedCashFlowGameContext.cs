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

        public virtual DbSet<Board> Boards { get; set; } = null!;
        public virtual DbSet<Dream> Dreams { get; set; } = null!;
        public virtual DbSet<EventCard> EventCards { get; set; } = null!;
        public virtual DbSet<FinancialAccount> FinancialAccounts { get; set; } = null!;
        public virtual DbSet<FinancialReport> FinancialReports { get; set; } = null!;
        public virtual DbSet<Friendship> Friendships { get; set; } = null!;
        public virtual DbSet<FriendshipStatus> FriendshipStatuses { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameAccount> GameAccounts { get; set; } = null!;
        public virtual DbSet<GameAccountType> GameAccountTypes { get; set; } = null!;
        public virtual DbSet<GameEvent> GameEvents { get; set; } = null!;
        public virtual DbSet<GameMatch> GameMatches { get; set; } = null!;
        public virtual DbSet<Inventory> Inventories { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<JobAccount> JobAccounts { get; set; } = null!;
        public virtual DbSet<JobCard> JobCards { get; set; } = null!;
        public virtual DbSet<Leaderboard> Leaderboards { get; set; } = null!;
        public virtual DbSet<LoginHistory> LoginHistories { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Tile> Tiles { get; set; } = null!;
        public virtual DbSet<TileType> TileTypes { get; set; } = null!;
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>(entity =>
            {
                entity.ToTable("Board");

                entity.Property(e => e.BoardId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("board_id")
                    .IsFixedLength();

                entity.Property(e => e.AmountFatTile).HasColumnName("amount_fat_tile");

                entity.Property(e => e.AmountRatTile).HasColumnName("amount_rat_tile");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.DementionBoard).HasColumnName("demention_board");

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("game_id")
                    .IsFixedLength();

                entity.Property(e => e.RadiusRatTile).HasColumnName("radius_rat_tile");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("update_by")
                    .IsFixedLength();

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Boards)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Board__game_id__6754599E");
            });

            modelBuilder.Entity<Dream>(entity =>
            {
                entity.ToTable("Dream");

                entity.Property(e => e.DreamId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("dream_id")
                    .IsFixedLength();

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.DreamImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("dream_image_url");

                entity.Property(e => e.DreamName)
                    .HasMaxLength(10)
                    .HasColumnName("dream_name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("update_by")
                    .IsFixedLength();
            });

            modelBuilder.Entity<EventCard>(entity =>
            {
                entity.ToTable("Event_card");

                entity.Property(e => e.EventCardId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("event_card_id")
                    .IsFixedLength();

                entity.Property(e => e.CardName)
                    .HasMaxLength(30)
                    .HasColumnName("card_name");

                entity.Property(e => e.CashFlow).HasColumnName("cash_flow");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.Dept).HasColumnName("dept");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.DownPay).HasColumnName("down_pay");

                entity.Property(e => e.EventId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("event_id")
                    .IsFixedLength();

                entity.Property(e => e.EventImageUrl)
                    .HasMaxLength(100)
                    .HasColumnName("event_image_url");

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("game_id")
                    .IsFixedLength();

                entity.Property(e => e.TradingRange)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("trading_range");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("update_by")
                    .IsFixedLength();

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventCards)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Event_car__event__6477ECF3");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.EventCards)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__Event_car__game___6383C8BA");
            });

            modelBuilder.Entity<FinancialAccount>(entity =>
            {
                entity.HasKey(e => new { e.FinacialId, e.GameAccountId })
                    .HasName("pk_FinacialAccount_id");

                entity.ToTable("Financial_account");

                entity.Property(e => e.FinacialId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("finacial_id")
                    .IsFixedLength();

                entity.Property(e => e.GameAccountId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("game_account_id")
                    .IsFixedLength();

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Finacial)
                    .WithMany(p => p.FinancialAccounts)
                    .HasForeignKey(d => d.FinacialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Finacial___finac__05D8E0BE");

                entity.HasOne(d => d.GameAccount)
                    .WithMany(p => p.FinancialAccounts)
                    .HasForeignKey(d => d.GameAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Finacial___game___06CD04F7");
            });

            modelBuilder.Entity<FinancialReport>(entity =>
            {
                entity.HasKey(e => e.FinacialId)
                    .HasName("PK__Finacial__717EA3A4D9B9F8F7");

                entity.ToTable("Financial_report");

                entity.Property(e => e.FinacialId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("finacial_id")
                    .IsFixedLength();

                entity.Property(e => e.ChildrenAmount).HasColumnName("children_amount");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.ExpensePerMonth).HasColumnName("expense_per_month");

                entity.Property(e => e.IncomePerMonth).HasColumnName("income_per_month");

                entity.Property(e => e.JobCardId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("job_card_id")
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("user_id")
                    .IsFixedLength();

                entity.HasOne(d => d.JobCard)
                    .WithMany(p => p.FinancialReports)
                    .HasForeignKey(d => d.JobCardId)
                    .HasConstraintName("FK__Finacial___job_c__797309D9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FinancialReports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Finacial___user___787EE5A0");
            });

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

            modelBuilder.Entity<GameAccount>(entity =>
            {
                entity.ToTable("Game_account");

                entity.Property(e => e.GameAccountId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("game_account_id")
                    .IsFixedLength();

                entity.Property(e => e.AccountTypeId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("account_type_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.GameAccountName)
                    .HasMaxLength(50)
                    .HasColumnName("game_account_name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("update_by")
                    .IsFixedLength();

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.GameAccounts)
                    .HasForeignKey(d => d.AccountTypeId)
                    .HasConstraintName("FK__Game_acco__accou__7F2BE32F");
            });

            modelBuilder.Entity<GameAccountType>(entity =>
            {
                entity.HasKey(e => e.AccountTypeId)
                    .HasName("PK__Game_acc__18186C10C9E35596");

                entity.ToTable("Game_account_type");

                entity.HasIndex(e => e.AccountTypeName, "UQ__Game_acc__2A5C6966FEC3691A")
                    .IsUnique();

                entity.Property(e => e.AccountTypeId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("account_type_id")
                    .IsFixedLength();

                entity.Property(e => e.AccountTypeName)
                    .HasMaxLength(10)
                    .HasColumnName("account_type_name");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("update_by")
                    .IsFixedLength();
            });

            modelBuilder.Entity<GameEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK__Game_eve__2370F727A91F09E9");

                entity.ToTable("Game_event");

                entity.Property(e => e.EventId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("event_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.EventName)
                    .HasMaxLength(30)
                    .HasColumnName("event_name");

                entity.Property(e => e.IsEventTile).HasColumnName("is_event_tile");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("update_by")
                    .IsFixedLength();
            });

            modelBuilder.Entity<GameMatch>(entity =>
            {
                entity.HasKey(e => e.MatchId)
                    .HasName("PK__Game_mat__9D7FCBA3D7973ACA");

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

                entity.Property(e => e.WinerId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("winer_id")
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

                entity.HasOne(d => d.Winer)
                    .WithMany(p => p.GameMatchWiners)
                    .HasForeignKey(d => d.WinerId)
                    .HasConstraintName("FK__Game_matc__winer__5812160E");
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
                    .HasMaxLength(100)
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

            modelBuilder.Entity<JobAccount>(entity =>
            {
                entity.HasKey(e => new { e.JobCardId, e.GameAccountId })
                    .HasName("pk_jobAccount_id");

                entity.ToTable("Job_account");

                entity.Property(e => e.JobCardId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("job_card_id")
                    .IsFixedLength();

                entity.Property(e => e.GameAccountId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("game_account_id")
                    .IsFixedLength();

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.GameAccount)
                    .WithMany(p => p.JobAccounts)
                    .HasForeignKey(d => d.GameAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Job_accou__game___02FC7413");

                entity.HasOne(d => d.JobCard)
                    .WithMany(p => p.JobAccounts)
                    .HasForeignKey(d => d.JobCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Job_accou__job_c__02084FDA");
            });

            modelBuilder.Entity<JobCard>(entity =>
            {
                entity.ToTable("Job_card");

                entity.Property(e => e.JobCardId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("job_card_id")
                    .IsFixedLength();

                entity.Property(e => e.ChildrenCost).HasColumnName("children_cost");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.JobImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("job_image_url");

                entity.Property(e => e.JobName)
                    .HasMaxLength(50)
                    .HasColumnName("job_name");

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

                entity.Property(e => e.TimeFeriod).HasColumnName("time_feriod");

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

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Position");

                entity.Property(e => e.PositionId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("position_id")
                    .IsFixedLength();

                entity.Property(e => e.TileId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("tile_id")
                    .IsFixedLength();

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Tile)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.TileId)
                    .HasConstraintName("FK__Position__tile_i__73BA3083");
            });

            modelBuilder.Entity<Tile>(entity =>
            {
                entity.ToTable("Tile");

                entity.Property(e => e.TileId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("tile_id")
                    .IsFixedLength();

                entity.Property(e => e.BoardId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("board_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.DreamId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("dream_id")
                    .IsFixedLength();

                entity.Property(e => e.EventId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("event_id")
                    .IsFixedLength();

                entity.Property(e => e.IsRatRace).HasColumnName("is_rat_race");

                entity.Property(e => e.TileTypeId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("tile_type_id")
                    .IsFixedLength();

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("update_by")
                    .IsFixedLength();

                entity.HasOne(d => d.Board)
                    .WithMany(p => p.Tiles)
                    .HasForeignKey(d => d.BoardId)
                    .HasConstraintName("FK__Tile__board_id__70DDC3D8");

                entity.HasOne(d => d.Dream)
                    .WithMany(p => p.Tiles)
                    .HasForeignKey(d => d.DreamId)
                    .HasConstraintName("FK__Tile__dream_id__6EF57B66");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Tiles)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Tile__event_id__6E01572D");

                entity.HasOne(d => d.TileType)
                    .WithMany(p => p.Tiles)
                    .HasForeignKey(d => d.TileTypeId)
                    .HasConstraintName("FK__Tile__tile_type___6FE99F9F");
            });

            modelBuilder.Entity<TileType>(entity =>
            {
                entity.ToTable("Tile_type");

                entity.Property(e => e.TileTypeId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("tile_type_id")
                    .IsFixedLength();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("create_by")
                    .IsFixedLength();

                entity.Property(e => e.TileTypeName)
                    .HasMaxLength(10)
                    .HasColumnName("tile_type_name");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("update_by")
                    .IsFixedLength();
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__User_acc__B9BE370FD1A1F24B");

                entity.ToTable("User_account");

                entity.HasIndex(e => e.NickName, "UQ__User_acc__08E8937AFC2E461A")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "UQ__User_acc__7C9273C4F0C1FA26")
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
                    .HasName("PK__User_rol__760965CC1AC66DE6");

                entity.ToTable("User_role");

                entity.HasIndex(e => e.RoleName, "UQ__User_rol__783254B10F8C8D55")
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
