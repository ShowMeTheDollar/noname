using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class OpenRoomContext : DbContext
{
    public OpenRoomContext()
    {
    }

    public OpenRoomContext(DbContextOptions<OpenRoomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AmentityType> AmentityTypes { get; set; }

    public virtual DbSet<Ecpay> Ecpays { get; set; }

    public virtual DbSet<LanguageSpeaker> LanguageSpeakers { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberThirdPartyLink> MemberThirdPartyLinks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomAmenity> RoomAmenities { get; set; }

    public virtual DbSet<RoomAmentityCategory> RoomAmentityCategories { get; set; }

    public virtual DbSet<RoomCategory> RoomCategories { get; set; }

    public virtual DbSet<RoomImage> RoomImages { get; set; }

    public virtual DbSet<RoomPrice> RoomPrices { get; set; }

    public virtual DbSet<RoomReview> RoomReviews { get; set; }

    public virtual DbSet<ThirdPartyLogin> ThirdPartyLogins { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    public virtual DbSet<WishlistDetail> WishlistDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:OpenRoomDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_Taiwan_Stroke_CI_AS");

        modelBuilder.Entity<AmentityType>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AmentityName).HasMaxLength(100);
        });

        modelBuilder.Entity<Ecpay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Green");

            entity.ToTable("Ecpay");

            entity.HasIndex(e => e.OrderId, "IX_Ecpay_OrderID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MerchantTradeNo).HasMaxLength(50);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.PaymentTypeChargeFee).HasMaxLength(50);
            entity.Property(e => e.RtnMsg).HasMaxLength(50);
            entity.Property(e => e.TradeDate).HasColumnType("datetime");
            entity.Property(e => e.TradeNo).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.Ecpays)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Green_Orders");
        });

        modelBuilder.Entity<LanguageSpeaker>(entity =>
        {
            entity.ToTable("LanguageSpeaker");

            entity.HasIndex(e => e.MemberId, "IX_LanguageSpeaker_MemberID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Member).WithMany(p => p.LanguageSpeakers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LanguageSpeaker_Members");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EditAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmergencyContact).HasMaxLength(100);
            entity.Property(e => e.EmergencyNumber).HasMaxLength(100);
            entity.Property(e => e.Job).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Live).HasMaxLength(100);
            entity.Property(e => e.Mobile).HasMaxLength(100);
            entity.Property(e => e.Obsession).HasMaxLength(100);
            entity.Property(e => e.Pet).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
        });

        modelBuilder.Entity<MemberThirdPartyLink>(entity =>
        {
            entity.ToTable("MemberThirdPartyLink");

            entity.HasIndex(e => e.MemberId, "IX_MemberThirdPartyLink_MemberID");

            entity.HasIndex(e => e.ThirdPartyId, "IX_MemberThirdPartyLink_ThirdPartyID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.ThirdPartyId).HasColumnName("ThirdPartyID");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberThirdPartyLinks)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberThirdPartyLink_Members");

            entity.HasOne(d => d.ThirdParty).WithMany(p => p.MemberThirdPartyLinks)
                .HasForeignKey(d => d.ThirdPartyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberThirdPartyLink_ThirdPartyLogin");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.MemberId, "IX_Orders_MemberID");

            entity.HasIndex(e => e.RoomId, "IX_Orders_RoomID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CheckIn).HasColumnType("datetime");
            entity.Property(e => e.CheckOut).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.OrderNo).HasMaxLength(50);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.TotalPrice).HasColumnType("money");
            entity.Property(e => e.UpdatedAt)
                .HasComment("只有變更才會有值")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Member).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Members");

            entity.HasOne(d => d.Room).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Rooms");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RoomSource");

            entity.HasIndex(e => e.MemberId, "IX_Rooms_MemberID");

            entity.HasIndex(e => e.RoomCategoryId, "IX_Rooms_RoomCategoryID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CheckInEndTime).HasColumnType("datetime");
            entity.Property(e => e.CheckInStartTime).HasColumnType("datetime");
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EditedAt).HasColumnType("datetime");
            entity.Property(e => e.FixedPrice).HasColumnType("money");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Note).HasComment("備註");
            entity.Property(e => e.RoomCategoryId).HasColumnName("RoomCategoryID");
            entity.Property(e => e.RoomName).HasMaxLength(100);
            entity.Property(e => e.Sort).HasComment("自定義排序");
            entity.Property(e => e.WeekendPrice).HasColumnType("money");

            entity.HasOne(d => d.Member).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rooms_Members");

            entity.HasOne(d => d.RoomCategory).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rooms_RoomCategories");
        });

        modelBuilder.Entity<RoomAmenity>(entity =>
        {
            entity.HasIndex(e => e.AmentityId, "IX_RoomAmenities_AmentityID");

            entity.HasIndex(e => e.RoomId, "IX_RoomAmenities_RoomID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AmentityId).HasColumnName("AmentityID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.Amentity).WithMany(p => p.RoomAmenities)
                .HasForeignKey(d => d.AmentityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomAmenities_RoomAmentityCategoryies");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomAmenities)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomAmenities_Rooms");
        });

        modelBuilder.Entity<RoomAmentityCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RoomAmentityCategoryies");

            entity.HasIndex(e => e.AmentityTypeId, "IX_RoomAmentityCategoryies_AmentityTypeID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AmentityName).HasMaxLength(100);
            entity.Property(e => e.AmentityTypeId).HasColumnName("AmentityTypeID");

            entity.HasOne(d => d.AmentityType).WithMany(p => p.RoomAmentityCategories)
                .HasForeignKey(d => d.AmentityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomAmentityCategoryies_AmentityTypes");
        });

        modelBuilder.Entity<RoomCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RoomSourceCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoomCategory1)
                .HasMaxLength(100)
                .HasColumnName("RoomCategory");
        });

        modelBuilder.Entity<RoomImage>(entity =>
        {
            entity.HasIndex(e => e.RoomId, "IX_RoomImages_RoomID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomImages)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomImages_Rooms");
        });

        modelBuilder.Entity<RoomPrice>(entity =>
        {
            entity.HasIndex(e => e.RoomId, "IX_RoomPrices_RoomID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.SeparatePrice).HasColumnType("money");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomPrices)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomPrices_Rooms");
        });

        modelBuilder.Entity<RoomReview>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.RoomReview)
                .HasForeignKey<RoomReview>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomReviews_Orders");
        });

        modelBuilder.Entity<ThirdPartyLogin>(entity =>
        {
            entity.ToTable("ThirdPartyLogin");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Provider).HasMaxLength(50);
            entity.Property(e => e.ProviderUserId)
                .HasMaxLength(100)
                .HasColumnName("ProviderUserID");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.ToTable("Wishlist");

            entity.HasIndex(e => e.MemberId, "IX_Wishlist_MemberID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.WishlistName).HasMaxLength(100);

            entity.HasOne(d => d.Member).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wishlist_Members");
        });

        modelBuilder.Entity<WishlistDetail>(entity =>
        {
            entity.HasIndex(e => e.RoomId, "IX_WishlistDetails_RoomID");

            entity.HasIndex(e => e.WishlistId, "IX_WishlistDetails_WishlistID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.WishlistId).HasColumnName("WishlistID");

            entity.HasOne(d => d.Room).WithMany(p => p.WishlistDetails)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WishlistDetails_Rooms");

            entity.HasOne(d => d.Wishlist).WithMany(p => p.WishlistDetails)
                .HasForeignKey(d => d.WishlistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WishlistDetails_Wishlist");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
