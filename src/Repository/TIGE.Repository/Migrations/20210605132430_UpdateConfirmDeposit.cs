using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class UpdateConfirmDeposit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FeeUSD",
                table: "UserWithDrawRequest",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmToken",
                table: "UserDepositRequest",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ConfirmedTime",
                table: "UserDepositRequest",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpireTime",
                table: "UserDepositRequest",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeeUSD",
                table: "UserWithDrawRequest");

            migrationBuilder.DropColumn(
                name: "ConfirmToken",
                table: "UserDepositRequest");

            migrationBuilder.DropColumn(
                name: "ConfirmedTime",
                table: "UserDepositRequest");

            migrationBuilder.DropColumn(
                name: "ExpireTime",
                table: "UserDepositRequest");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "User");
        }
    }
}
