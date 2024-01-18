﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class addDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 18, 15, 22, 47, 355, DateTimeKind.Local).AddTicks(9896), new DateTime(2024, 1, 18, 15, 22, 47, 355, DateTimeKind.Local).AddTicks(9897) });

            migrationBuilder.UpdateData(
                table: "InventoryThresholds",
                keyColumn: "Id",
                keyValue: new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 18, 15, 22, 47, 355, DateTimeKind.Local).AddTicks(9930), new DateTime(2024, 1, 18, 15, 22, 47, 355, DateTimeKind.Local).AddTicks(9930) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 17, 19, 58, 51, 369, DateTimeKind.Local).AddTicks(9368), new DateTime(2024, 1, 17, 19, 58, 51, 369, DateTimeKind.Local).AddTicks(9369) });

            migrationBuilder.UpdateData(
                table: "InventoryThresholds",
                keyColumn: "Id",
                keyValue: new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 17, 19, 58, 51, 369, DateTimeKind.Local).AddTicks(9397), new DateTime(2024, 1, 17, 19, 58, 51, 369, DateTimeKind.Local).AddTicks(9397) });
        }
    }
}
