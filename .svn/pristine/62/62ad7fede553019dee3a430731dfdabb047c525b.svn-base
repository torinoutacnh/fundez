using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class StackUpdate7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TxHash",
                table: "HistoryWithdrawToken",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TxHash",
                table: "HistoryWithdrawToken");
        }
    }
}
