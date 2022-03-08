using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class UpdateConfigChart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TIGEChartRange",
                table: "Configuration",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TIGEChartRange",
                table: "Configuration");
        }
    }
}
