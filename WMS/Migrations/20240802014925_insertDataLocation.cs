using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class insertDataLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 2, 8, 49, 24, 897, DateTimeKind.Local).AddTicks(1640), new DateTime(2024, 8, 2, 8, 49, 24, 897, DateTimeKind.Local).AddTicks(1640) });

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

            migrationBuilder.InsertData(
                table: "StockLocation",
                columns: new[] { "Id", "Active", "Barcode", "CompleteName", "CreateDate", "CreateUid", "LocationId", "Name", "ParentPath", "RemovalStrategyId", "Usage", "WriteDate", "WriteUid" },
                values: new object[,]
                {
                    { new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), true, null, "Partners", new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(360), null, null, "Partners", "f47ac10b-58cc-4372-a567-0e02b2c3d479/", null, 0, new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(360), null },
                    { new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"), true, null, "Partners / Vendors", new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(370), null, new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), "Vendors", "f47ac10b-58cc-4372-a567-0e02b2c3d479/6ba7b810-9dad-11d1-80b4-00c04fd430c8/", null, 3, new DateTime(2024, 8, 2, 8, 49, 24, 898, DateTimeKind.Local).AddTicks(380), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"));

            migrationBuilder.DeleteData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 1, 8, 56, 3, 138, DateTimeKind.Local).AddTicks(7660), new DateTime(2024, 8, 1, 8, 56, 3, 138, DateTimeKind.Local).AddTicks(7670) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6130), new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6140) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6180), new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6180) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6200), new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6200) });
        }
    }
}
