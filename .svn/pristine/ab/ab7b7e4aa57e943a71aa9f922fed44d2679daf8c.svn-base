using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class UpdateSlotWithBusiness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromUserSlotId",
                table: "UserBusiness",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBusiness_FromUserSlotId",
                table: "UserBusiness",
                column: "FromUserSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBusiness_UserSlots_FromUserSlotId",
                table: "UserBusiness",
                column: "FromUserSlotId",
                principalTable: "UserSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBusiness_UserSlots_FromUserSlotId",
                table: "UserBusiness");

            migrationBuilder.DropIndex(
                name: "IX_UserBusiness_FromUserSlotId",
                table: "UserBusiness");

            migrationBuilder.DropColumn(
                name: "FromUserSlotId",
                table: "UserBusiness");
        }
    }
}
