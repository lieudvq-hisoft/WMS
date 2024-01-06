using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateInventory1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Inventory",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 6, 10, 33, 23, 152, DateTimeKind.Local).AddTicks(8830), new DateTime(2024, 1, 6, 10, 33, 23, 152, DateTimeKind.Local).AddTicks(8830) });

            migrationBuilder.InsertData(
                table: "InventoryThresholds",
                columns: new[] { "Id", "CronExpression", "DateCreated", "DateUpdated", "IsDeleted", "ThresholdQuantity" },
                values: new object[] { new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"), "20 16 * * *", new DateTime(2024, 1, 6, 10, 33, 23, 152, DateTimeKind.Local).AddTicks(8860), new DateTime(2024, 1, 6, 10, 33, 23, 152, DateTimeKind.Local).AddTicks(8860), false, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InventoryThresholds",
                keyColumn: "Id",
                keyValue: new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Inventory");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 4, 15, 56, 51, 592, DateTimeKind.Local).AddTicks(9540), new DateTime(2024, 1, 4, 15, 56, 51, 592, DateTimeKind.Local).AddTicks(9540) });
        }
    }
}
