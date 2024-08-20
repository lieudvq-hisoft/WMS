using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateStockLot1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockLot_StockLocation_LocationId",
                table: "StockLot");

            migrationBuilder.DropIndex(
                name: "IX_StockLot_LocationId",
                table: "StockLot");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "StockLot");

            migrationBuilder.AddColumn<Guid>(
                name: "LotId",
                table: "StockMoveLine",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_StockMoveLine_LotId",
                table: "StockMoveLine",
                column: "LotId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMoveLine_StockLot_LotId",
                table: "StockMoveLine",
                column: "LotId",
                principalTable: "StockLot",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMoveLine_StockLot_LotId",
                table: "StockMoveLine");

            migrationBuilder.DropIndex(
                name: "IX_StockMoveLine_LotId",
                table: "StockMoveLine");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "StockMoveLine");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "StockLot",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_StockLot_LocationId",
                table: "StockLot",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockLot_StockLocation_LocationId",
                table: "StockLot",
                column: "LocationId",
                principalTable: "StockLocation",
                principalColumn: "Id");
        }
    }
}
