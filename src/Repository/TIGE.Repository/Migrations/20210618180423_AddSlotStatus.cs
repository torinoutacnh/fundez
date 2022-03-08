using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class AddSlotStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserSlots",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserSlots");
        }
    }
}
