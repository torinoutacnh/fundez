using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class AddSlotAndToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Slots",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Tokens",
                table: "User",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "TokenChanges",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    Rate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenChanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSlots",
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
                    Quantity = table.Column<int>(nullable: false),
                    TokenQuantity = table.Column<double>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    CurrentBalance = table.Column<double>(nullable: false),
                    ApproveTime = table.Column<DateTimeOffset>(nullable: true),
                    ApproveBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSlots_User_ApproveBy",
                        column: x => x.ApproveBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSlots_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
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
                    SlotId = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_UserSlots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "UserSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSlots_ApproveBy",
                table: "UserSlots",
                column: "ApproveBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserSlots_CurrentBalance",
                table: "UserSlots",
                column: "CurrentBalance");

            migrationBuilder.CreateIndex(
                name: "IX_UserSlots_DeletedTime",
                table: "UserSlots",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserSlots_Quantity",
                table: "UserSlots",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_UserSlots_UserId",
                table: "UserSlots",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_DeletedTime",
                table: "UserTokens",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_Quantity",
                table: "UserTokens",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_Rate",
                table: "UserTokens",
                column: "Rate");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_SlotId",
                table: "UserTokens",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_TotalAmount",
                table: "UserTokens",
                column: "TotalAmount");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenChanges");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "UserSlots");

            migrationBuilder.DropColumn(
                name: "Slots",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Tokens",
                table: "User");
        }
    }
}
