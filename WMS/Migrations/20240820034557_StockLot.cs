using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class StockLot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "StockLot",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StockLot",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "StockLot",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "StockLot",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_StockLot_LocationId",
                table: "StockLot",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLot_ProductId",
                table: "StockLot",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockLot_ProductProduct_ProductId",
                table: "StockLot",
                column: "ProductId",
                principalTable: "ProductProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockLot_StockLocation_LocationId",
                table: "StockLot",
                column: "LocationId",
                principalTable: "StockLocation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockLot_ProductProduct_ProductId",
                table: "StockLot");

            migrationBuilder.DropForeignKey(
                name: "FK_StockLot_StockLocation_LocationId",
                table: "StockLot");

            migrationBuilder.DropIndex(
                name: "IX_StockLot_LocationId",
                table: "StockLot");

            migrationBuilder.DropIndex(
                name: "IX_StockLot_ProductId",
                table: "StockLot");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "StockLot");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StockLot");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "StockLot");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "StockLot");

            migrationBuilder.DropColumn(
                name: "Tracking",
                table: "ProductProduct");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 51, 21, 616, DateTimeKind.Local).AddTicks(7360), new DateTime(2024, 8, 10, 14, 51, 21, 616, DateTimeKind.Local).AddTicks(7360) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6840), new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6840) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6820), new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6820) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6710), new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6740) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6780), new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6780) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6800), new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6800) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6810), new DateTime(2024, 8, 10, 14, 51, 21, 617, DateTimeKind.Local).AddTicks(6810) });
        }
    }
}
