﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TIGE.Repository;

namespace TIGE.Repository.Migrations
{
    [DbContext(typeof(TigeDbContext))]
    [Migration("20210530133657_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TIGE.Contract.Repository.Models.Application.RefreshTokenEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("AccuracyRadius")
                        .HasColumnType("int");

                    b.Property<string>("BrowserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrowserVersion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContinentCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryIsoCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeviceHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EngineName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EngineVersion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("ExpireOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("IpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("MarkerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MarkerVersion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OsName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OsVersion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TimeZone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalUsage")
                        .HasColumnType("int");

                    b.Property<string>("UserAgent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("RefreshToken");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.ConfigurationEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AboutPage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConfigMail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConfigMailPass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactMails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FactoryAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FactoryAddress2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FactoryAddress3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HelloPage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<double>("LatLocation")
                        .HasColumnType("float");

                    b.Property<double>("LongLocation")
                        .HasColumnType("float");

                    b.Property<string>("MainEMail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Partner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SalePhone1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SalePhone2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TitleInMail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ZoomRate")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Configuration");
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.ImageEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("ContentLength")
                        .HasColumnType("bigint");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageDominantHexColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ImageHeightPx")
                        .HasColumnType("int");

                    b.Property<int>("ImageWidthPx")
                        .HasColumnType("int");

                    b.Property<bool>("IsFeatured")
                        .HasColumnType("bit");

                    b.Property<bool>("IsImage")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastAccessTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MimeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginalImageHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StorageLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TotalAccess")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.User.UserDepositRequestEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AmountETH")
                        .HasColumnType("float");

                    b.Property<double>("AmountUSD")
                        .HasColumnType("float");

                    b.Property<string>("ApproveBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproveId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TxHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WalletId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ApproveId");

                    b.HasIndex("WalletId");

                    b.ToTable("UserDepositRequest");
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.User.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AboutMe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BannedRemark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("BannedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConfirmEmailToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("ConfirmEmailTokenExpireTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("EmailConfirmedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityCardNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("PasswordLastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Permission")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("PhoneConfirmedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ReferenceId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SetPasswordToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("SetPasswordTokenExpireTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("Email");

                    b.HasIndex("Phone");

                    b.HasIndex("ReferenceId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.User.UserWalletEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AddressWallet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("AmountUSD")
                        .HasColumnType("float");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastWebHookNotify")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PrivateKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WebHook")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebHookId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserWallet");
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.User.UserWithDrawRequestEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AmountETH")
                        .HasColumnType("float");

                    b.Property<double>("AmountUSD")
                        .HasColumnType("float");

                    b.Property<string>("ApproveById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("ApproveTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ConfirmToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("ConfirmedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("ExpireTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FromWalletId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<string>("RejectById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RejectReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("RejectTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TxHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WalletId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AmountETH");

                    b.HasIndex("AmountUSD");

                    b.HasIndex("ApproveById");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("FromWalletId");

                    b.HasIndex("Rate");

                    b.HasIndex("RejectById");

                    b.HasIndex("WalletId");

                    b.ToTable("UserWithDrawRequest");
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.Application.RefreshTokenEntity", b =>
                {
                    b.HasOne("TIGE.Contract.Repository.Models.User.UserEntity", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.User.UserDepositRequestEntity", b =>
                {
                    b.HasOne("TIGE.Contract.Repository.Models.User.UserEntity", "Approve")
                        .WithMany()
                        .HasForeignKey("ApproveId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TIGE.Contract.Repository.Models.User.UserWalletEntity", "Wallet")
                        .WithMany("DepositRequests")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.User.UserEntity", b =>
                {
                    b.HasOne("TIGE.Contract.Repository.Models.User.UserEntity", "Reference")
                        .WithMany()
                        .HasForeignKey("ReferenceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.User.UserWalletEntity", b =>
                {
                    b.HasOne("TIGE.Contract.Repository.Models.User.UserEntity", "User")
                        .WithMany("Wallets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TIGE.Contract.Repository.Models.User.UserWithDrawRequestEntity", b =>
                {
                    b.HasOne("TIGE.Contract.Repository.Models.User.UserEntity", "ApproveBy")
                        .WithMany("ApproveRequest")
                        .HasForeignKey("ApproveById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TIGE.Contract.Repository.Models.User.UserEntity", "RejectBy")
                        .WithMany("RejectRequest")
                        .HasForeignKey("RejectById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TIGE.Contract.Repository.Models.User.UserWalletEntity", "Wallet")
                        .WithMany("WithdrawRequests")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}