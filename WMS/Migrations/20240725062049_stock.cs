using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class stock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    RemovalStrategyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CompleteName = table.Column<string>(type: "text", nullable: true),
                    ParentPath = table.Column<string>(type: "text", nullable: true),
                    Barcode = table.Column<string>(type: "text", nullable: true),
                    Usage = table.Column<int>(type: "integer", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockLocation_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockLocation_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockLocation_ProductRemoval_RemovalStrategyId",
                        column: x => x.RemovalStrategyId,
                        principalTable: "ProductRemoval",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockLocation_StockLocation_LocationId",
                        column: x => x.LocationId,
                        principalTable: "StockLocation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockWarehouse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ViewLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    LotStockId = table.Column<Guid>(type: "uuid", nullable: false),
                    WhInputStockLocId = table.Column<Guid>(type: "uuid", nullable: true),
                    WhQcStockLocId = table.Column<Guid>(type: "uuid", nullable: true),
                    WhOutputStockLocId = table.Column<Guid>(type: "uuid", nullable: true),
                    WhPackStockLocId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockWarehouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockWarehouse_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockWarehouse_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockWarehouse_StockLocation_LotStockId",
                        column: x => x.LotStockId,
                        principalTable: "StockLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockWarehouse_StockLocation_ViewLocationId",
                        column: x => x.ViewLocationId,
                        principalTable: "StockLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockWarehouse_StockLocation_WhInputStockLocId",
                        column: x => x.WhInputStockLocId,
                        principalTable: "StockLocation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockWarehouse_StockLocation_WhOutputStockLocId",
                        column: x => x.WhOutputStockLocId,
                        principalTable: "StockLocation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockWarehouse_StockLocation_WhPackStockLocId",
                        column: x => x.WhPackStockLocId,
                        principalTable: "StockLocation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockWarehouse_StockLocation_WhQcStockLocId",
                        column: x => x.WhQcStockLocId,
                        principalTable: "StockLocation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockPickingType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReturnPickingTypeTd = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Barcode = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreateBackorder = table.Column<int>(type: "integer", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPickingType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockPickingType_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockPickingType_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockPickingType_StockPickingType_ReturnPickingTypeTd",
                        column: x => x.ReturnPickingTypeTd,
                        principalTable: "StockPickingType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockPickingType_StockWarehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "StockWarehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 25, 13, 20, 49, 762, DateTimeKind.Local).AddTicks(660), new DateTime(2024, 7, 25, 13, 20, 49, 762, DateTimeKind.Local).AddTicks(660) });

            migrationBuilder.CreateIndex(
                name: "IX_StockLocation_CreateUid",
                table: "StockLocation",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockLocation_LocationId",
                table: "StockLocation",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLocation_RemovalStrategyId",
                table: "StockLocation",
                column: "RemovalStrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLocation_WriteUid",
                table: "StockLocation",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockPickingType_CreateUid",
                table: "StockPickingType",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockPickingType_ReturnPickingTypeTd",
                table: "StockPickingType",
                column: "ReturnPickingTypeTd");

            migrationBuilder.CreateIndex(
                name: "IX_StockPickingType_WarehouseId",
                table: "StockPickingType",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockPickingType_WriteUid",
                table: "StockPickingType",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockWarehouse_CreateUid",
                table: "StockWarehouse",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockWarehouse_LotStockId",
                table: "StockWarehouse",
                column: "LotStockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockWarehouse_ViewLocationId",
                table: "StockWarehouse",
                column: "ViewLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockWarehouse_WhInputStockLocId",
                table: "StockWarehouse",
                column: "WhInputStockLocId");

            migrationBuilder.CreateIndex(
                name: "IX_StockWarehouse_WhOutputStockLocId",
                table: "StockWarehouse",
                column: "WhOutputStockLocId");

            migrationBuilder.CreateIndex(
                name: "IX_StockWarehouse_WhPackStockLocId",
                table: "StockWarehouse",
                column: "WhPackStockLocId");

            migrationBuilder.CreateIndex(
                name: "IX_StockWarehouse_WhQcStockLocId",
                table: "StockWarehouse",
                column: "WhQcStockLocId");

            migrationBuilder.CreateIndex(
                name: "IX_StockWarehouse_WriteUid",
                table: "StockWarehouse",
                column: "WriteUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockPickingType");

            migrationBuilder.DropTable(
                name: "StockWarehouse");

            migrationBuilder.DropTable(
                name: "StockLocation");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 15, 14, 34, 28, 706, DateTimeKind.Local).AddTicks(6190), new DateTime(2024, 7, 15, 14, 34, 28, 706, DateTimeKind.Local).AddTicks(6190) });
        }
    }
}
