using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class UpdateVerifyCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmToken",
                table: "UserSlots",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ConfirmedTime",
                table: "UserSlots",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpireTime",
                table: "UserSlots",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmToken",
                table: "UserSellToken",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ConfirmedTime",
                table: "UserSellToken",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpireTime",
                table: "UserSellToken",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmToken",
                table: "UserSlots");

            migrationBuilder.DropColumn(
                name: "ConfirmedTime",
                table: "UserSlots");

            migrationBuilder.DropColumn(
                name: "ExpireTime",
                table: "UserSlots");

            migrationBuilder.DropColumn(
                name: "ConfirmToken",
                table: "UserSellToken");

            migrationBuilder.DropColumn(
                name: "ConfirmedTime",
                table: "UserSellToken");

            migrationBuilder.DropColumn(
                name: "ExpireTime",
                table: "UserSellToken");
        }
    }
}
