using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class FK_StockMove_ProductUomId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMove_UomUom_ProductUomId",
                table: "StockMove");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 4, 10, 42, 58, 325, DateTimeKind.Local).AddTicks(4540), new DateTime(2024, 8, 4, 10, 42, 58, 325, DateTimeKind.Local).AddTicks(4540) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3850), new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3850) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3750), new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3760) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3810), new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3820), new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3830) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3840), new DateTime(2024, 8, 4, 10, 42, 58, 326, DateTimeKind.Local).AddTicks(3840) });

            migrationBuilder.AddForeignKey(
                name: "FK_StockMove_UomUom_ProductUomId",
                table: "StockMove",
                column: "ProductUomId",
                principalTable: "UomUom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMove_UomUom_ProductUomId",
                table: "StockMove");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 2, 8, 49, 24, 897, DateTimeKind.Local).AddTicks(1640), new DateTime(2024, 8, 2, 8, 49, 24, 897, DateTimeKind.Local).AddTicks(1640) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(370), new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(380) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(280), new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(290) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(330), new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(340) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(350), new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(350) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(360), new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(360) });

            migrationBuilder.AddForeignKey(
                name: "FK_StockMove_UomUom_ProductUomId",
                table: "StockMove",
                column: "ProductUomId",
                principalTable: "UomUom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
