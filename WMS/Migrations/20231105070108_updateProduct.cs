using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "Product",
                newName: "SalePrice");

            migrationBuilder.AddColumn<string>(
                name: "InternalCode",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Product",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalCode",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "Product",
                newName: "UnitPrice");

            migrationBuilder.AddColumn<double>(
                name: "CostPrice",
                table: "Product",
                type: "double precision",
                nullable: true);
        }
    }
}
