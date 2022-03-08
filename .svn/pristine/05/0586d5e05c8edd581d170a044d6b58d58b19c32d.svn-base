using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TIGE.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    AboutPage = table.Column<string>(nullable: true),
                    HelloPage = table.Column<string>(nullable: true),
                    Partner = table.Column<string>(nullable: true),
                    ContactMails = table.Column<string>(nullable: true),
                    MainEMail = table.Column<string>(nullable: true),
                    SalePhone1 = table.Column<string>(nullable: true),
                    SalePhone2 = table.Column<string>(nullable: true),
                    FactoryAddress = table.Column<string>(nullable: true),
                    FactoryAddress2 = table.Column<string>(nullable: true),
                    FactoryAddress3 = table.Column<string>(nullable: true),
                    CompanyAddress = table.Column<string>(nullable: true),
                    LatLocation = table.Column<double>(nullable: false),
                    LongLocation = table.Column<double>(nullable: false),
                    ZoomRate = table.Column<double>(nullable: false),
                    TitleInMail = table.Column<string>(nullable: true),
                    ConfigMail = table.Column<string>(nullable: true),
                    ConfigMailPass = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    StorageLocation = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    MimeType = table.Column<string>(nullable: true),
                    IsImage = table.Column<bool>(nullable: false),
                    IsFeatured = table.Column<bool>(nullable: false),
                    ContentLength = table.Column<long>(nullable: false),
                    TotalAccess = table.Column<long>(nullable: false),
                    LastAccessTime = table.Column<DateTimeOffset>(nullable: false),
                    ImageDominantHexColor = table.Column<string>(nullable: true),
                    ImageWidthPx = table.Column<int>(nullable: false),
                    ImageHeightPx = table.Column<int>(nullable: false),
                    OriginalImageHash = table.Column<string>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PasswordLastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    PhoneConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmEmailToken = table.Column<string>(nullable: true),
                    ConfirmEmailTokenExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    SetPasswordToken = table.Column<string>(nullable: true),
                    SetPasswordTokenExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    BannedTime = table.Column<DateTimeOffset>(nullable: true),
                    BannedRemark = table.Column<string>(nullable: true),
                    Permission = table.Column<string>(nullable: true),
                    AboutMe = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    AvatarUrl = table.Column<string>(nullable: true),
                    IdentityCardNo = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    ReferenceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_User_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    ExpireOn = table.Column<DateTimeOffset>(nullable: true),
                    TotalUsage = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    MarkerName = table.Column<string>(nullable: true),
                    MarkerVersion = table.Column<string>(nullable: true),
                    OsName = table.Column<string>(nullable: true),
                    OsVersion = table.Column<string>(nullable: true),
                    EngineName = table.Column<string>(nullable: true),
                    EngineVersion = table.Column<string>(nullable: true),
                    BrowserName = table.Column<string>(nullable: true),
                    BrowserVersion = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    CountryIsoCode = table.Column<string>(nullable: true),
                    ContinentCode = table.Column<string>(nullable: true),
                    TimeZone = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    AccuracyRadius = table.Column<int>(nullable: true),
                    DeviceType = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    DeviceHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserWallet",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    AddressWallet = table.Column<string>(nullable: true),
                    PrivateKey = table.Column<string>(nullable: true),
                    PublicKey = table.Column<string>(nullable: true),
                    AmountUSD = table.Column<double>(nullable: false),
                    WebHook = table.Column<string>(nullable: true),
                    WebHookId = table.Column<string>(nullable: true),
                    LastWebHookNotify = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWallet_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDepositRequest",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    WalletId = table.Column<string>(nullable: true),
                    AmountETH = table.Column<double>(nullable: false),
                    AmountUSD = table.Column<double>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    TxHash = table.Column<string>(nullable: true),
                    ApproveBy = table.Column<string>(nullable: true),
                    ApproveId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDepositRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDepositRequest_User_ApproveId",
                        column: x => x.ApproveId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDepositRequest_UserWallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "UserWallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserWithDrawRequest",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    FromWalletId = table.Column<string>(nullable: true),
                    WalletId = table.Column<string>(nullable: true),
                    AmountETH = table.Column<double>(nullable: false),
                    AmountUSD = table.Column<double>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    TxHash = table.Column<string>(nullable: true),
                    ApproveById = table.Column<string>(nullable: true),
                    ApproveTime = table.Column<DateTimeOffset>(nullable: true),
                    RejectById = table.Column<string>(nullable: true),
                    RejectReason = table.Column<string>(nullable: true),
                    RejectTime = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    ExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmToken = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWithDrawRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWithDrawRequest_User_ApproveById",
                        column: x => x.ApproveById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserWithDrawRequest_User_RejectById",
                        column: x => x.RejectById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserWithDrawRequest_UserWallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "UserWallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_DeletedTime",
                table: "Image",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_DeletedTime",
                table: "RefreshToken",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_RefreshToken",
                table: "RefreshToken",
                column: "RefreshToken");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedTime",
                table: "User",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_User_Phone",
                table: "User",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_User_ReferenceId",
                table: "User",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepositRequest_ApproveId",
                table: "UserDepositRequest",
                column: "ApproveId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepositRequest_WalletId",
                table: "UserDepositRequest",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWallet_UserId",
                table: "UserWallet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithDrawRequest_AmountETH",
                table: "UserWithDrawRequest",
                column: "AmountETH");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithDrawRequest_AmountUSD",
                table: "UserWithDrawRequest",
                column: "AmountUSD");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithDrawRequest_ApproveById",
                table: "UserWithDrawRequest",
                column: "ApproveById");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithDrawRequest_DeletedTime",
                table: "UserWithDrawRequest",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithDrawRequest_FromWalletId",
                table: "UserWithDrawRequest",
                column: "FromWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithDrawRequest_Rate",
                table: "UserWithDrawRequest",
                column: "Rate");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithDrawRequest_RejectById",
                table: "UserWithDrawRequest",
                column: "RejectById");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithDrawRequest_WalletId",
                table: "UserWithDrawRequest",
                column: "WalletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "UserDepositRequest");

            migrationBuilder.DropTable(
                name: "UserWithDrawRequest");

            migrationBuilder.DropTable(
                name: "UserWallet");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
