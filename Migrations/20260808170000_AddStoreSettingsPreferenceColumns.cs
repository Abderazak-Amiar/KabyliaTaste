using KabyliaTaste.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KabyliaTaste.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260808170000_AddStoreSettingsPreferenceColumns")]
    public partial class AddStoreSettingsPreferenceColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "StoreSettings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "StoreSettings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductUnitsJson",
                table: "StoreSettings",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "ProductUnitsJson",
                table: "StoreSettings");
        }
    }
}
