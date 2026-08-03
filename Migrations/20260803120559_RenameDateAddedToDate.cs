using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KabyliaTaste.Migrations
{
    /// <inheritdoc />
    public partial class RenameDateAddedToDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateAdded",
                table: "Products",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Products",
                newName: "DateAdded");
        }
    }
}
