using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky_app.Migrations
{
    /// <inheritdoc />
    public partial class RenameIdPropertyOfProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Products",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "Key");
        }
    }
}
