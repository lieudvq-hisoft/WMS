using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class serialHandel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Location_LocationId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_LocationId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Inventory");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "Product",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Inventory",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SerialCode",
                table: "Inventory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 4, 14, 5, 31, 702, DateTimeKind.Local).AddTicks(6400), new DateTime(2023, 12, 4, 14, 5, 31, 702, DateTimeKind.Local).AddTicks(6400) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "SerialCode",
                table: "Inventory");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Inventory",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 11, 29, 15, 50, 33, 285, DateTimeKind.Local).AddTicks(6540), new DateTime(2023, 11, 29, 15, 50, 33, 285, DateTimeKind.Local).AddTicks(6550) });

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_LocationId",
                table: "Inventory",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Location_LocationId",
                table: "Inventory",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }
    }
}
