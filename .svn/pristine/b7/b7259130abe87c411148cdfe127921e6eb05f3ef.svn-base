using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class StackUpdate9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoryWithdrawUSD",
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
                    TigePrice = table.Column<double>(nullable: false),
                    AmountUSD = table.Column<double>(nullable: false),
                    FeeUSD = table.Column<double>(nullable: false),
                    TxHash = table.Column<string>(nullable: true),
                    Rate = table.Column<double>(nullable: false),
                    ToWalletAddress = table.Column<string>(nullable: true),
                    ConfirmToken = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ApproveBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryWithdrawUSD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryWithdrawUSD_StackingWallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "StackingWallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoryWithdrawUSD_WalletId",
                table: "HistoryWithdrawUSD",
                column: "WalletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryWithdrawUSD");
        }
    }
}
