using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class stockLocationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockWarehouse_StockLocation_LotStockId",
                table: "StockWarehouse");

            migrationBuilder.DropForeignKey(
                name: "FK_StockWarehouse_StockLocation_ViewLocationId",
                table: "StockWarehouse");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 44, 7, 313, DateTimeKind.Local).AddTicks(6690), new DateTime(2024, 8, 10, 14, 44, 7, 313, DateTimeKind.Local).AddTicks(6690) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7440), new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7450) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7430), new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7430) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7310), new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7320) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7390), new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7390) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7410), new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7410) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7420), new DateTime(2024, 8, 10, 14, 44, 7, 317, DateTimeKind.Local).AddTicks(7420) });

            migrationBuilder.AddForeignKey(
                name: "FK_StockWarehouse_StockLocation_LotStockId",
                table: "StockWarehouse",
                column: "LotStockId",
                principalTable: "StockLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockWarehouse_StockLocation_ViewLocationId",
                table: "StockWarehouse",
                column: "ViewLocationId",
                principalTable: "StockLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockWarehouse_StockLocation_LotStockId",
                table: "StockWarehouse");

            migrationBuilder.DropForeignKey(
                name: "FK_StockWarehouse_StockLocation_ViewLocationId",
                table: "StockWarehouse");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 8, 11, 35, 9, 122, DateTimeKind.Local).AddTicks(3300), new DateTime(2024, 8, 8, 11, 35, 9, 122, DateTimeKind.Local).AddTicks(3300) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2630), new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2630) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2620), new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2620) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2520), new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2520) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2570), new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2590), new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2600) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2610), new DateTime(2024, 8, 8, 11, 35, 9, 123, DateTimeKind.Local).AddTicks(2610) });

            migrationBuilder.AddForeignKey(
                name: "FK_StockWarehouse_StockLocation_LotStockId",
                table: "StockWarehouse",
                column: "LotStockId",
                principalTable: "StockLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockWarehouse_StockLocation_ViewLocationId",
                table: "StockWarehouse",
                column: "ViewLocationId",
                principalTable: "StockLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
