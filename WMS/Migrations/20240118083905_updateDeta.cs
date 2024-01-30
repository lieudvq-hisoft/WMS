using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateDeta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 18, 15, 39, 5, 836, DateTimeKind.Local).AddTicks(2859), new DateTime(2024, 1, 18, 15, 39, 5, 836, DateTimeKind.Local).AddTicks(2861) });

            migrationBuilder.UpdateData(
                table: "InventoryThresholds",
                keyColumn: "Id",
                keyValue: new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 18, 15, 39, 5, 836, DateTimeKind.Local).AddTicks(2967), new DateTime(2024, 1, 18, 15, 39, 5, 836, DateTimeKind.Local).AddTicks(2967) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 18, 15, 34, 27, 251, DateTimeKind.Local).AddTicks(2275), new DateTime(2024, 1, 18, 15, 34, 27, 251, DateTimeKind.Local).AddTicks(2275) });

            migrationBuilder.UpdateData(
                table: "InventoryThresholds",
                keyColumn: "Id",
                keyValue: new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 18, 15, 34, 27, 251, DateTimeKind.Local).AddTicks(2301), new DateTime(2024, 1, 18, 15, 34, 27, 251, DateTimeKind.Local).AddTicks(2302) });
        }
    }
}
