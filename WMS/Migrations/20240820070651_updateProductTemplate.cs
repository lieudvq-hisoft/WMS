using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateProductTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tracking",
                table: "ProductProduct");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Tracking",
            //    table: "ProductTemplate",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "text");

            migrationBuilder.Sql(
            "ALTER TABLE \"ProductTemplate\" " +
            "ALTER COLUMN \"Tracking\" TYPE integer " +
            "USING CASE " +
            "WHEN \"Tracking\" = 'None' THEN 0 " +
            "WHEN \"Tracking\" = 'Serial' THEN 1 " +
            "WHEN \"Tracking\" = 'Lot' THEN 2 " +
            "ELSE 0 " +
            "END;");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 20, 14, 6, 51, 231, DateTimeKind.Local).AddTicks(2040), new DateTime(2024, 8, 20, 14, 6, 51, 231, DateTimeKind.Local).AddTicks(2040) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1750), new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1750) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1740), new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1740) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1640), new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1650) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1700), new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1700) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1720), new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1720) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1730), new DateTime(2024, 8, 20, 14, 6, 51, 232, DateTimeKind.Local).AddTicks(1730) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tracking",
                table: "ProductTemplate",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Tracking",
                table: "ProductProduct",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 20, 10, 45, 56, 880, DateTimeKind.Local).AddTicks(6040), new DateTime(2024, 8, 20, 10, 45, 56, 880, DateTimeKind.Local).AddTicks(6040) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5070), new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5070) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5060), new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5060) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(4970), new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(4980) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5020), new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5020) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5040), new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5040) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5050), new DateTime(2024, 8, 20, 10, 45, 56, 881, DateTimeKind.Local).AddTicks(5050) });
        }
    }
}
