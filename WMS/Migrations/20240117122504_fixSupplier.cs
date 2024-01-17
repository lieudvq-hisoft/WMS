﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class fixSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Supplier",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffName",
                table: "Supplier",
                type: "text",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "StaffName",
                table: "Supplier");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 17, 19, 19, 23, 908, DateTimeKind.Local).AddTicks(3822), new DateTime(2024, 1, 17, 19, 19, 23, 908, DateTimeKind.Local).AddTicks(3823) });

            migrationBuilder.UpdateData(
                table: "InventoryThresholds",
                keyColumn: "Id",
                keyValue: new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 17, 19, 19, 23, 908, DateTimeKind.Local).AddTicks(3856), new DateTime(2024, 1, 17, 19, 19, 23, 908, DateTimeKind.Local).AddTicks(3857) });
        }
    }
}
