using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UomUom_UomCategory_CategoryId",
                table: "UomUom");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 13, 20, 57, 51, 671, DateTimeKind.Local).AddTicks(4510), new DateTime(2024, 7, 13, 20, 57, 51, 671, DateTimeKind.Local).AddTicks(4520) });

            migrationBuilder.AddForeignKey(
                name: "FK_UomUom_UomCategory_CategoryId",
                table: "UomUom",
                column: "CategoryId",
                principalTable: "UomCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UomUom_UomCategory_CategoryId",
                table: "UomUom");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 9, 14, 45, 50, 861, DateTimeKind.Local).AddTicks(190), new DateTime(2024, 7, 9, 14, 45, 50, 861, DateTimeKind.Local).AddTicks(190) });

            migrationBuilder.AddForeignKey(
                name: "FK_UomUom_UomCategory_CategoryId",
                table: "UomUom",
                column: "CategoryId",
                principalTable: "UomCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
