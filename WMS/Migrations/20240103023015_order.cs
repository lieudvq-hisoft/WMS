using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PickingRequest_AspNetUsers_SentBy",
                table: "PickingRequest");

            migrationBuilder.RenameColumn(
                name: "SentBy",
                table: "PickingRequest",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PickingRequest_SentBy",
                table: "PickingRequest",
                newName: "IX_PickingRequest_OrderId");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SentBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Files = table.Column<List<string>>(type: "text[]", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_SentBy",
                        column: x => x.SentBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickingRequestUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PickingRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickingRequestUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickingRequestUser_AspNetUsers_ReceivedBy",
                        column: x => x.ReceivedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickingRequestUser_PickingRequest_PickingRequestId",
                        column: x => x.PickingRequestId,
                        principalTable: "PickingRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 1, 3, 9, 30, 15, 461, DateTimeKind.Local).AddTicks(1930), new DateTime(2024, 1, 3, 9, 30, 15, 461, DateTimeKind.Local).AddTicks(1930) });

            migrationBuilder.CreateIndex(
                name: "IX_Order_SentBy",
                table: "Order",
                column: "SentBy");

            migrationBuilder.CreateIndex(
                name: "IX_PickingRequestUser_PickingRequestId",
                table: "PickingRequestUser",
                column: "PickingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingRequestUser_ReceivedBy",
                table: "PickingRequestUser",
                column: "ReceivedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_PickingRequest_Order_OrderId",
                table: "PickingRequest",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PickingRequest_Order_OrderId",
                table: "PickingRequest");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "PickingRequestUser");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "PickingRequest",
                newName: "SentBy");

            migrationBuilder.RenameIndex(
                name: "IX_PickingRequest_OrderId",
                table: "PickingRequest",
                newName: "IX_PickingRequest_SentBy");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 4, 14, 12, 45, 598, DateTimeKind.Local).AddTicks(3750), new DateTime(2023, 12, 4, 14, 12, 45, 598, DateTimeKind.Local).AddTicks(3750) });

            migrationBuilder.AddForeignKey(
                name: "FK_PickingRequest_AspNetUsers_SentBy",
                table: "PickingRequest",
                column: "SentBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
