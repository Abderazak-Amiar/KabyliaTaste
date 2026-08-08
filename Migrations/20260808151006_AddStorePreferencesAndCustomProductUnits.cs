using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KabyliaTaste.Migrations
{
    /// <inheritdoc />
    public partial class AddStorePreferencesAndCustomProductUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "Products");
        }
    }
}
