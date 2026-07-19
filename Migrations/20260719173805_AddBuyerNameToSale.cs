using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KabyliaTaste.Migrations
{
    /// <inheritdoc />
    public partial class AddBuyerNameToSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "Sales",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "Sales");
        }
    }
}
