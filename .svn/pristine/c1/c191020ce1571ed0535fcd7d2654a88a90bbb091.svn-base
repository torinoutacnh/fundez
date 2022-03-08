using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class UpdateConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyAddress",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "ConfigMail",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "ConfigMailPass",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "ContactMails",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "FactoryAddress",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "FactoryAddress2",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "FactoryAddress3",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "LatLocation",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "LongLocation",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "MainEMail",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "Partner",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "TitleInMail",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "ZoomRate",
                table: "Configuration");

            migrationBuilder.AddColumn<string>(
                name: "ContactAddress",
                table: "Configuration",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SlotPrice",
                table: "Configuration",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SlotToToken",
                table: "Configuration",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TokenPrice",
                table: "Configuration",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "TotalDepositWallet",
                table: "Configuration",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactAddress",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "SlotPrice",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "SlotToToken",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "TokenPrice",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "TotalDepositWallet",
                table: "Configuration");

            migrationBuilder.AddColumn<string>(
                name: "CompanyAddress",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfigMail",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfigMailPass",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactMails",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FactoryAddress",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FactoryAddress2",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FactoryAddress3",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LatLocation",
                table: "Configuration",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LongLocation",
                table: "Configuration",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "MainEMail",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Partner",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleInMail",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoomRate",
                table: "Configuration",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
