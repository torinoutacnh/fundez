using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class StackUpdate5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactAddress",
                table: "StackingConfig",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupportEmail",
                table: "StackingConfig",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TIGEChartRange",
                table: "StackingConfig",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TelegramLink",
                table: "StackingConfig",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactAddress",
                table: "StackingConfig");

            migrationBuilder.DropColumn(
                name: "SupportEmail",
                table: "StackingConfig");

            migrationBuilder.DropColumn(
                name: "TIGEChartRange",
                table: "StackingConfig");

            migrationBuilder.DropColumn(
                name: "TelegramLink",
                table: "StackingConfig");
        }
    }
}
