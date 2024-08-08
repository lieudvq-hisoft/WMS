using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class imageUrlPtmp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ProductTemplate",
                type: "text",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ProductTemplate");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 6, 8, 45, 8, 971, DateTimeKind.Local).AddTicks(2740), new DateTime(2024, 8, 6, 8, 45, 8, 971, DateTimeKind.Local).AddTicks(2740) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2520), new DateTime(2024, 8, 6, 8, 45, 8, 972, DateTimeKind.Local).AddTicks(2520) });

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
        }
    }
}
