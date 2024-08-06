using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class stockLocationCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 6, 8, 45, 8, 971, DateTimeKind.Local).AddTicks(2740), new DateTime(2024, 8, 6, 8, 45, 8, 971, DateTimeKind.Local).AddTicks(2740) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2500), new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2500) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2400), new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2410) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2460), new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2460) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2480), new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2480) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2490), new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2490) });

            migrationBuilder.InsertData(
                table: "StockLocation",
                columns: new[] { "Id", "Active", "Barcode", "CompleteName", "CreateDate", "CreateUid", "LocationId", "Name", "ParentPath", "RemovalStrategyId", "Usage", "WriteDate", "WriteUid" },
                values: new object[] { new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"), true, null, "Partners / Customers", new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2520), null, new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), "Customers", "f47ac10b-58cc-4372-a567-0e02b2c3d479/6ba7b180-9cad-11d1-80b4-00c04fd430c8/", null, 4, new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2520), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"));

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
        }
    }
}
