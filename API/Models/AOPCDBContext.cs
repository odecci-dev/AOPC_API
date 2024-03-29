﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using CMS.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class AOPCDBContext : DbContext
{
    public AOPCDBContext(DbContextOptions<AOPCDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<TblApiTokenModel> TblApiTokenModels { get; set; }

    public virtual DbSet<TblAudittrailModel> TblAudittrailModels { get; set; }

    public virtual DbSet<TblBusinessLocationModel> TblBusinessLocationModels { get; set; }

    public virtual DbSet<TblBusinessModel> TblBusinessModels { get; set; }

    public virtual DbSet<TblBusinessTypeModel> TblBusinessTypeModels { get; set; }

    public virtual DbSet<TblCorporateModel> TblCorporateModels { get; set; }

    public virtual DbSet<TblCorporatePrivilegeLogsModel> TblCorporatePrivilegeLogsModels { get; set; }

    public virtual DbSet<TblCorporatePrivilegeModel> TblCorporatePrivilegeModels { get; set; }

    public virtual DbSet<TblCorporatePrivilegeTierModel> TblCorporatePrivilegeTierModels { get; set; }

    public virtual DbSet<TblCorporatePrivilegesModel> TblCorporatePrivilegesModels { get; set; }

    public virtual DbSet<TblDeletedUserModel> TblDeletedUserModels { get; set; }

    public virtual DbSet<TblMembershipModel> TblMembershipModels { get; set; }

    public virtual DbSet<TblMembershipPrivilegeModel> TblMembershipPrivilegeModels { get; set; }

    public virtual DbSet<TblNotificationModel> TblNotificationModels { get; set; }

    public virtual DbSet<TblOfferingModel> TblOfferingModels { get; set; }

    public virtual DbSet<TblOfferingsModel> TblOfferingsModels { get; set; }

    public virtual DbSet<TblPositionModel> TblPositionModels { get; set; }

    public virtual DbSet<TblPrivilegeModel> TblPrivilegeModels { get; set; }

    public virtual DbSet<TblQrCodeLogsModel> TblQrCodeLogsModels { get; set; }

    public virtual DbSet<TblRegistrationOtpmodel> TblRegistrationOtpmodels { get; set; }

    public virtual DbSet<TblStatusModel> TblStatusModels { get; set; }

    public virtual DbSet<TblSupportModel> TblSupportModels { get; set; }

    public virtual DbSet<TblTokenModel> TblTokenModels { get; set; }

    public virtual DbSet<TblUserMembershipModel> TblUserMembershipModels { get; set; }

    public virtual DbSet<TblUserPrivilegeModel> TblUserPrivilegeModels { get; set; }

    public virtual DbSet<TblUserTypeModel> TblUserTypeModels { get; set; }

    public virtual DbSet<TblVendorModel> TblVendorModels { get; set; }

    public virtual DbSet<TblVouchersModel> TblVouchersModels { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<UsersModel> UsersModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.Property(e => e.RoleId).IsRequired();

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.CompanyName)
                .IsRequired()
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);
            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<TblApiTokenModel>(entity =>
        {
            entity.ToTable("tbl_ApiTokenModel");

            entity.Property(e => e.ApiToken).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.Role).IsUnicode(false);
        });

        modelBuilder.Entity<TblAudittrailModel>(entity =>
        {
            entity.ToTable("tbl_audittrailModel");

            entity.HasIndex(e => e.Module, "idx_category_group");

            entity.HasIndex(e => new { e.Business, e.DateCreated }, "idx_mail_group");

            entity.Property(e => e.ActionId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("ActionID");
            entity.Property(e => e.Actions)
                .HasMaxLength(1700)
                .IsUnicode(false);
            entity.Property(e => e.Business)
                .HasMaxLength(1700)
                .IsUnicode(false);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(1700)
                .IsUnicode(false)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Module)
                .HasMaxLength(1700)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TblBusinessLocationModel>(entity =>
        {
            entity.ToTable("tbl_BusinessLocationModel");

            entity.Property(e => e.BusinessLocId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasComputedColumnSql("((('BLOC'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false)
                .HasColumnName("BusinessLocID");
            entity.Property(e => e.City).IsUnicode(false);
            entity.Property(e => e.Country).IsUnicode(false);
            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.PostalCode).IsUnicode(false);
        });

        modelBuilder.Entity<TblBusinessModel>(entity =>
        {
            entity.ToTable("tbl_BusinessModel");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.BusinessId)
                .HasMaxLength(37)
                .IsUnicode(false)
                .HasComputedColumnSql("((('Hotel'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false)
                .HasColumnName("BusinessID");
            entity.Property(e => e.BusinessName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Cno).IsUnicode(false);
            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.FeatureImg).IsUnicode(false);
            entity.Property(e => e.FilePath).IsUnicode(false);
            entity.Property(e => e.Gallery).IsUnicode(false);
            entity.Property(e => e.Map).IsUnicode(false);
            entity.Property(e => e.Services).IsUnicode(false);
            entity.Property(e => e.Url).IsUnicode(false);
        });

        modelBuilder.Entity<TblBusinessTypeModel>(entity =>
        {
            entity.ToTable("tbl_BusinessTypeModel");

            entity.Property(e => e.BusinessTypeId)
                .HasMaxLength(37)
                .IsUnicode(false)
                .HasComputedColumnSql("((('BTYPE'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false)
                .HasColumnName("BusinessTypeID");
            entity.Property(e => e.BusinessTypeName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ImgUrl)
                .IsUnicode(false)
                .HasColumnName("ImgURL");
            entity.Property(e => e.IsVip).HasColumnName("isVIP");
            entity.Property(e => e.PromoText).IsUnicode(false);
        });

        modelBuilder.Entity<TblCorporateModel>(entity =>
        {
            entity.ToTable("tbl_CorporateModel");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Cno)
                .IsUnicode(false)
                .HasColumnName("CNo");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasComputedColumnSql("((('Cmp'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.CorporateName).IsUnicode(false);
            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.DateEnded).HasColumnType("datetime");
            entity.Property(e => e.DateUsed).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).IsUnicode(false);
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
        });

        modelBuilder.Entity<TblCorporatePrivilegeLogsModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_PrivilegeLogsModel");

            entity.ToTable("tbl_CorporatePrivilegeLogsModel");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateRedeemed).HasColumnType("datetime");
            entity.Property(e => e.RedeemedBy)
                .IsRequired()
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblCorporatePrivilegeModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_CorporatePrivilege");

            entity.ToTable("tbl_CorporatePrivilegeModel");

            entity.Property(e => e.DateIssued).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblCorporatePrivilegeTierModel>(entity =>
        {
            entity.ToTable("tbl_CorporatePrivilegeTierModel");

            entity.Property(e => e.CorporateId).HasColumnName("CorporateID");
            entity.Property(e => e.Count)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.IsVip).HasColumnName("isVIP");
            entity.Property(e => e.PrivilegeId).HasColumnName("PrivilegeID");
            entity.Property(e => e.VipCount)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblCorporatePrivilegesModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_PrivilegesModel");

            entity.ToTable("tbl_CorporatePrivilegesModel");

            entity.Property(e => e.CorporateId).HasColumnName("CorporateID");
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
        });

        modelBuilder.Entity<TblDeletedUserModel>(entity =>
        {
            entity.ToTable("tbl_DeletedUserModel");

            entity.Property(e => e.DateDeleted)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.Reason)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserEmail)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblMembershipModel>(entity =>
        {
            entity.ToTable("tbl_MembershipModel");

            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.DateEnded).HasColumnType("datetime");
            entity.Property(e => e.DateUsed).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.MembershipCard)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MembershipId)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasComputedColumnSql("((('MEM'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false)
                .HasColumnName("MembershipID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Qrframe)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("QRFrame");
            entity.Property(e => e.Vipbadge)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VIPBadge");
            entity.Property(e => e.Vipcard)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VIPCard");
            entity.Property(e => e.Vipcount).HasColumnName("VIPCount");
        });

        modelBuilder.Entity<TblMembershipPrivilegeModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_MembershipPrivilege");

            entity.ToTable("tbl_MembershipPrivilegeModel");

            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.PrivilegeId).HasColumnName("PrivilegeID");
        });

        modelBuilder.Entity<TblNotificationModel>(entity =>
        {
            entity.ToTable("tbl_NotificationModel");

            entity.Property(e => e.DateCreated).HasColumnType("date");
            entity.Property(e => e.Details).IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .IsUnicode(false)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.IsRead).HasColumnName("isRead");
            entity.Property(e => e.ItemId)
                .IsUnicode(false)
                .HasColumnName("ItemID");
            entity.Property(e => e.Module).IsUnicode(false);
        });

        modelBuilder.Entity<TblOfferingModel>(entity =>
        {
            entity.ToTable("tbl_OfferingModel");

            entity.Property(e => e.BusinessTypeId).HasColumnName("BusinessTypeID");
            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.FromTime).IsUnicode(false);
            entity.Property(e => e.ImgUrl).IsUnicode(false);
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.OfferDays).IsUnicode(false);
            entity.Property(e => e.OfferingId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasComputedColumnSql("((('Offering'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false)
                .HasColumnName("OfferingID");
            entity.Property(e => e.OfferingName).IsUnicode(false);
            entity.Property(e => e.PrivilegeId).HasColumnName("PrivilegeID");
            entity.Property(e => e.PromoReleaseText).IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.ToTime).IsUnicode(false);
            entity.Property(e => e.Url).IsUnicode(false);
            entity.Property(e => e.VendorId).HasColumnName("VendorID");
        });

        modelBuilder.Entity<TblOfferingsModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_Offerings");

            entity.ToTable("tbl_OfferingsModel");

            entity.Property(e => e.BusinessTypeId).HasColumnName("BusinessTypeID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDatetime).HasColumnType("datetime");
            entity.Property(e => e.Expiry).HasColumnType("datetime");
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.OfferDays).IsUnicode(false);
            entity.Property(e => e.OfferingDesc).HasColumnType("text");
            entity.Property(e => e.PrivilegeId).HasColumnName("PrivilegeID");
            entity.Property(e => e.PromoDesc).HasColumnType("text");
            entity.Property(e => e.PromoReleaseText).HasColumnType("text");
            entity.Property(e => e.StartDatetime).HasColumnType("datetime");
            entity.Property(e => e.Url).IsUnicode(false);
        });

        modelBuilder.Entity<TblPositionModel>(entity =>
        {
            entity.ToTable("tbl_PositionModel");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.PositionId)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasComputedColumnSql("((('POS'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false)
                .HasColumnName("PositionID");
        });

        modelBuilder.Entity<TblPrivilegeModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_UserPrivilegeModel");

            entity.ToTable("tbl_PrivilegeModel");

            entity.Property(e => e.BusinessTypeId).HasColumnName("BusinessTypeID");
            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ImgUrl).IsUnicode(false);
            entity.Property(e => e.IsVip).HasColumnName("isVIP");
            entity.Property(e => e.Mechanics).IsUnicode(false);
            entity.Property(e => e.PrivilegeId)
                .HasMaxLength(41)
                .IsUnicode(false)
                .HasComputedColumnSql("((('Privilege'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false)
                .HasColumnName("PrivilegeID");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.Tmc)
                .IsUnicode(false)
                .HasColumnName("TMC");
            entity.Property(e => e.Validity).HasColumnType("datetime");
            entity.Property(e => e.VendorId).HasColumnName("VendorID");
        });

        modelBuilder.Entity<TblQrCodeLogsModel>(entity =>
        {
            entity.ToTable("tbl_QrCodeLogsModel");

            entity.Property(e => e.AreaCode).IsUnicode(false);
            entity.Property(e => e.City).IsUnicode(false);
            entity.Property(e => e.Country).IsUnicode(false);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .IsUnicode(false)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Ipaddres)
                .IsUnicode(false)
                .HasColumnName("IPAddres");
            entity.Property(e => e.Isocode)
                .IsUnicode(false)
                .HasColumnName("ISOCode");
            entity.Property(e => e.Latitude).IsUnicode(false);
            entity.Property(e => e.Longtitude).IsUnicode(false);
            entity.Property(e => e.MetroCode).IsUnicode(false);
            entity.Property(e => e.PostalCode).IsUnicode(false);
            entity.Property(e => e.Region).IsUnicode(false);
            entity.Property(e => e.TimeZone).IsUnicode(false);
            entity.Property(e => e.ZipCode).IsUnicode(false);
        });

        modelBuilder.Entity<TblRegistrationOtpmodel>(entity =>
        {
            entity.ToTable("tbl_RegistrationOTPModel");

            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.Otp)
                .IsUnicode(false)
                .HasColumnName("OTP");
        });

        modelBuilder.Entity<TblStatusModel>(entity =>
        {
            entity.ToTable("tbl_StatusModel");

            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblSupportModel>(entity =>
        {
            entity.ToTable("tbl_SupportModel");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .IsUnicode(false)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Message).IsUnicode(false);
        });

        modelBuilder.Entity<TblTokenModel>(entity =>
        {
            entity.ToTable("tbl_TokenModel");

            entity.Property(e => e.DateCreated).HasColumnType("date");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("expiryDate");
            entity.Property(e => e.Token).IsUnicode(false);
        });

        modelBuilder.Entity<TblUserMembershipModel>(entity =>
        {
            entity.ToTable("tbl_UserMembershipModel");

            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.MembershipNumber)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasComputedColumnSql("((('MEM'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Validity).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblUserPrivilegeModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_UserPrivilegeLogsModel");

            entity.ToTable("tbl_UserPrivilegeModel");

            entity.Property(e => e.DateCreated)
                .HasComputedColumnSql("(getdate())", false)
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Validity).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblUserTypeModel>(entity =>
        {
            entity.ToTable("tbl_UserTypeModel");

            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.UserType).IsUnicode(false);
        });

        modelBuilder.Entity<TblVendorModel>(entity =>
        {
            entity.ToTable("tbl_VendorModel");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.BusinessLocationId)
                .IsUnicode(false)
                .HasColumnName("BusinessLocationID");
            entity.Property(e => e.Cno).IsUnicode(false);
            entity.Property(e => e.DateCreated).HasColumnType("date");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.FeatureImg).IsUnicode(false);
            entity.Property(e => e.FileUrl).IsUnicode(false);
            entity.Property(e => e.Gallery).IsUnicode(false);
            entity.Property(e => e.Map).IsUnicode(false);
            entity.Property(e => e.Services).IsUnicode(false);
            entity.Property(e => e.VendorId)
                .HasMaxLength(38)
                .IsUnicode(false)
                .HasComputedColumnSql("((('Vendor'+'-')+'0')+CONVERT([varchar],[Id],(0)))", false)
                .HasColumnName("VendorID");
            entity.Property(e => e.VendorLogo).IsUnicode(false);
            entity.Property(e => e.VendorName)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.VideoUrl).IsUnicode(false);
            entity.Property(e => e.VrUrl).IsUnicode(false);
            entity.Property(e => e.WebsiteUrl).IsUnicode(false);
        });

        modelBuilder.Entity<TblVouchersModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_Vouchers");

            entity.ToTable("tbl_VouchersModel");

            entity.Property(e => e.Count).HasColumnType("text");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Expiry).HasColumnType("datetime");
            entity.Property(e => e.VoucherName).IsUnicode(false);
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Test1);

            entity.ToTable("test");

            entity.Property(e => e.Test1).HasColumnName("test");
            entity.Property(e => e.Qweqwe)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("qweqwe");
            entity.Property(e => e.Testid)
                .HasMaxLength(38)
                .IsUnicode(false)
                .HasComputedColumnSql("((('Vendor'+'-')+'0')+CONVERT([varchar],[test],(0)))", false)
                .HasColumnName("testid");
            entity.Property(e => e.Testname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("testname");
        });

        modelBuilder.Entity<UsersModel>(entity =>
        {
            entity.ToTable("UsersModel");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Cno)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CorporateId).HasColumnName("CorporateID");
            entity.Property(e => e.DateCreated).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Fname).IsUnicode(false);
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsVip).HasColumnName("isVIP");
            entity.Property(e => e.Jwtoken)
                .IsUnicode(false)
                .HasColumnName("JWToken");
            entity.Property(e => e.Lname).IsUnicode(false);
            entity.Property(e => e.Mname).IsUnicode(false);
            entity.Property(e => e.Otp)
                .IsUnicode(false)
                .HasColumnName("OTP");
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingGeneratedProcedures(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}