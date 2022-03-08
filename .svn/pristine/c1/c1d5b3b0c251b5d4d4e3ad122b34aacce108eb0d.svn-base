using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class AddUserBusiness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBusiness",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    FromUserId = table.Column<string>(nullable: true),
                    ToUserId = table.Column<string>(nullable: true),
                    AmountUSD = table.Column<double>(nullable: false),
                    AmountSlot = table.Column<int>(nullable: false),
                    Commission = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBusiness", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBusiness_User_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserBusiness_User_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBusiness_AmountUSD",
                table: "UserBusiness",
                column: "AmountUSD");

            migrationBuilder.CreateIndex(
                name: "IX_UserBusiness_DeletedTime",
                table: "UserBusiness",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserBusiness_FromUserId",
                table: "UserBusiness",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBusiness_ToUserId",
                table: "UserBusiness",
                column: "ToUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBusiness");
        }
    }
}
