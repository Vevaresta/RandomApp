using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomApp.ProductManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currenty",
                table: "Products",
                newName: "Currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Products",
                newName: "Currenty");
        }
    }
}
