using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class dataStockLocationView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(3310), new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(3310) });

            migrationBuilder.InsertData(
                table: "StockLocation",
                columns: new[] { "Id", "Active", "Barcode", "CompleteName", "CreateDate", "CreateUid", "LocationId", "Name", "ParentPath", "RemovalStrategyId", "Usage", "WriteDate", "WriteUid" },
                values: new object[,]
                {
                    { new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"), true, null, "Virtual Locations", new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8640), null, null, "Virtual Locations", "b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071/", null, 0, new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8640), null },
                    { new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"), true, null, "Physical Locations", new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8700), null, null, "Physical Locations", "e2a7c3e0-1a4d-43b6-95e1-123456789abc/", null, 0, new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8700), null },
                    { new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"), true, null, "Virtual Locations / Inventory adjustment", new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8690), null, new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"), "Inventory adjustment", "b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071/d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6/", null, 0, new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8690), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"));

            migrationBuilder.DeleteData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"));

            migrationBuilder.DeleteData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 25, 13, 20, 49, 762, DateTimeKind.Local).AddTicks(660), new DateTime(2024, 7, 25, 13, 20, 49, 762, DateTimeKind.Local).AddTicks(660) });
        }
    }
}
