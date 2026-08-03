using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KabyliaTaste.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleDriveSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleDriveClientId",
                table: "StoreSettings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleDriveClientSecret",
                table: "StoreSettings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleDriveFolderId",
                table: "StoreSettings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleDriveRefreshToken",
                table: "StoreSettings",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleDriveClientId",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "GoogleDriveClientSecret",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "GoogleDriveFolderId",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "GoogleDriveRefreshToken",
                table: "StoreSettings");
        }
    }
}
