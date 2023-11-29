using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Location_LocationId",
                table: "Inventory");

            migrationBuilder.AlterColumn<Guid>(
                name: "LocationId",
                table: "Inventory",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "InventoryLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryLocation_Inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 11, 29, 15, 50, 33, 285, DateTimeKind.Local).AddTicks(6540), new DateTime(2023, 11, 29, 15, 50, 33, 285, DateTimeKind.Local).AddTicks(6550) });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLocation_InventoryId",
                table: "InventoryLocation",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLocation_LocationId",
                table: "InventoryLocation",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Location_LocationId",
                table: "Inventory",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Location_LocationId",
                table: "Inventory");

            migrationBuilder.DropTable(
                name: "InventoryLocation");

            migrationBuilder.AlterColumn<Guid>(
                name: "LocationId",
                table: "Inventory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 11, 29, 10, 13, 37, 771, DateTimeKind.Local).AddTicks(7390), new DateTime(2023, 11, 29, 10, 13, 37, 771, DateTimeKind.Local).AddTicks(7390) });

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Location_LocationId",
                table: "Inventory",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
