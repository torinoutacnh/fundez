using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class AddProtectionFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProtectionFee",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    From = table.Column<double>(nullable: false),
                    To = table.Column<double>(nullable: false),
                    Fee = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionFee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSellToken",
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
                    TokenQuantity = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    FeeAmount = table.Column<double>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false),
                    ApproveTime = table.Column<DateTimeOffset>(nullable: true),
                    ApproveBy = table.Column<string>(nullable: true),
                    ApproveId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSellToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSellToken_User_ApproveId",
                        column: x => x.ApproveId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSellToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSellToken_ApproveBy",
                table: "UserSellToken",
                column: "ApproveBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserSellToken_ApproveId",
                table: "UserSellToken",
                column: "ApproveId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSellToken_DeletedTime",
                table: "UserSellToken",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserSellToken_UserId",
                table: "UserSellToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProtectionFee");

            migrationBuilder.DropTable(
                name: "UserSellToken");
        }
    }
}
