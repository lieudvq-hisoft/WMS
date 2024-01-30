using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class fixUserv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "roleID",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated", "roleID" },
                values: new object[] { new DateTime(2024, 1, 17, 19, 54, 14, 923, DateTimeKind.Local).AddTicks(6596), new DateTime(2024, 1, 17, 19, 54, 14, 923, DateTimeKind.Local).AddTicks(6598), null });

            migrationBuilder.UpdateData(
                table: "InventoryThresholds",
                keyColumn: "Id",
                keyValue: new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 17, 19, 54, 14, 923, DateTimeKind.Local).AddTicks(6630), new DateTime(2024, 1, 17, 19, 54, 14, 923, DateTimeKind.Local).AddTicks(6630) });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_roleID",
                table: "AspNetUsers",
                column: "roleID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_roleID",
                table: "AspNetUsers",
                column: "roleID",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_roleID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_roleID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "roleID",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 17, 19, 25, 4, 267, DateTimeKind.Local).AddTicks(2020), new DateTime(2024, 1, 17, 19, 25, 4, 267, DateTimeKind.Local).AddTicks(2021) });

            migrationBuilder.UpdateData(
                table: "InventoryThresholds",
                keyColumn: "Id",
                keyValue: new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 17, 19, 25, 4, 267, DateTimeKind.Local).AddTicks(2052), new DateTime(2024, 1, 17, 19, 25, 4, 267, DateTimeKind.Local).AddTicks(2052) });
        }
    }
}
