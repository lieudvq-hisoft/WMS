using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updatePR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseUnitPrice",
                table: "Receipt",
                newName: "CostPrice");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Receipt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PickingRequest",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PickingRequest");

            migrationBuilder.RenameColumn(
                name: "CostPrice",
                table: "Receipt",
                newName: "PurchaseUnitPrice");
        }
    }
}
