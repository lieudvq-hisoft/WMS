using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class StockPickingFilePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "FilePaths",
                table: "StockPicking",
                type: "text[]",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 9, 27, 21, 22, 3, 925, DateTimeKind.Local).AddTicks(6630), new DateTime(2024, 9, 27, 21, 22, 3, 925, DateTimeKind.Local).AddTicks(6630) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6070), new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6070) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6060), new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6060) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(5970), new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(5970) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6020), new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6020) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6040), new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6040) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6050), new DateTime(2024, 9, 27, 21, 22, 3, 926, DateTimeKind.Local).AddTicks(6050) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePaths",
                table: "StockPicking");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 20, 15, 12, 29, 889, DateTimeKind.Local).AddTicks(6460), new DateTime(2024, 8, 20, 15, 12, 29, 889, DateTimeKind.Local).AddTicks(6470) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6270), new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6270) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6260), new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6260) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6160), new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6160) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6220), new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6220) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6240), new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6240) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6250), new DateTime(2024, 8, 20, 15, 12, 29, 890, DateTimeKind.Local).AddTicks(6250) });
        }
    }
}
