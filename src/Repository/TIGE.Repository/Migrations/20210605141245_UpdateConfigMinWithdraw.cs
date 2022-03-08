﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class UpdateConfigMinWithdraw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MinWithdraw",
                table: "Configuration",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinWithdraw",
                table: "Configuration");
        }
    }
}
