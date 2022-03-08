using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class AddStack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StackingConfig",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    AboutPage = table.Column<string>(nullable: true),
                    HelloPage = table.Column<string>(nullable: true),
                    SalePhone1 = table.Column<string>(nullable: true),
                    SalePhone2 = table.Column<string>(nullable: true),
                    DepositAmount = table.Column<double>(nullable: false),
                    MinStacking = table.Column<double>(nullable: false),
                    MinWithDraw = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackingConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StackingWallet",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    WalletAddress = table.Column<string>(nullable: true),
                    Balance = table.Column<double>(nullable: false),
                    TotalReward = table.Column<double>(nullable: false),
                    DailyReward = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackingWallet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    Day = table.Column<int>(nullable: false),
                    Reward = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoryDeposit",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    WalletId = table.Column<string>(nullable: true),
                    AmountTige = table.Column<double>(nullable: false),
                    TxHash = table.Column<string>(nullable: true),
                    Rate = table.Column<double>(nullable: false),
                    ConfirmToken = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    DayEnd = table.Column<DateTimeOffset>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryDeposit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryDeposit_StackingWallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "StackingWallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoryRefund",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    WalletId = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Rate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryRefund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryRefund_StackingWallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "StackingWallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoryWithdrawToken",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    WalletId = table.Column<string>(nullable: true),
                    AmountTige = table.Column<double>(nullable: false),
                    FeeTige = table.Column<double>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    ToWalletAddress = table.Column<string>(nullable: true),
                    ConfirmToken = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryWithdrawToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryWithdrawToken_StackingWallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "StackingWallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferToken",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    WalletId = table.Column<string>(nullable: true),
                    AmountTige = table.Column<double>(nullable: false),
                    FeeTige = table.Column<double>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    ConfirmToken = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferToken_StackingWallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "StackingWallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StackHistory",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    WalletId = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    SubscriptionDetail = table.Column<string>(nullable: true),
                    StackAmount = table.Column<double>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    DailyReward = table.Column<double>(nullable: false),
                    TotalReward = table.Column<double>(nullable: false),
                    ConfirmToken = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DateEnd = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StackHistory_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StackHistory_StackingWallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "StackingWallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoryDeposit_WalletId",
                table: "HistoryDeposit",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryRefund_WalletId",
                table: "HistoryRefund",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryWithdrawToken_WalletId",
                table: "HistoryWithdrawToken",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_StackHistory_SubscriptionId",
                table: "StackHistory",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_StackHistory_WalletId",
                table: "StackHistory",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferToken_WalletId",
                table: "TransferToken",
                column: "WalletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryDeposit");

            migrationBuilder.DropTable(
                name: "HistoryRefund");

            migrationBuilder.DropTable(
                name: "HistoryWithdrawToken");

            migrationBuilder.DropTable(
                name: "StackHistory");

            migrationBuilder.DropTable(
                name: "StackingConfig");

            migrationBuilder.DropTable(
                name: "TransferToken");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "StackingWallet");
        }
    }
}
