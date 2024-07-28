using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class stockMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockLot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockLot_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockLot_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockPicking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPicking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockPicking_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockPicking_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockQuant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    LotId = table.Column<Guid>(type: "uuid", nullable: true),
                    InventoryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    InventoryQuantity = table.Column<int>(type: "integer", nullable: true),
                    InventoryDiffQuantity = table.Column<int>(type: "integer", nullable: true),
                    InventoryQuantitySet = table.Column<bool>(type: "boolean", nullable: true),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockQuant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockQuant_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockQuant_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockQuant_ProductProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockQuant_StockLocation_LocationId",
                        column: x => x.LocationId,
                        principalTable: "StockLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockQuant_StockLot_LotId",
                        column: x => x.LotId,
                        principalTable: "StockLot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockMove",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductUomId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationDestId = table.Column<Guid>(type: "uuid", nullable: false),
                    PickingId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: true),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    DescriptionPicking = table.Column<string>(type: "text", nullable: true),
                    ProductQty = table.Column<int>(type: "integer", nullable: false),
                    ProductUomQty = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMove", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMove_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMove_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMove_ProductProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMove_StockLocation_LocationDestId",
                        column: x => x.LocationDestId,
                        principalTable: "StockLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMove_StockLocation_LocationId",
                        column: x => x.LocationId,
                        principalTable: "StockLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMove_StockPicking_PickingId",
                        column: x => x.PickingId,
                        principalTable: "StockPicking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMove_UomUom_ProductUomId",
                        column: x => x.ProductUomId,
                        principalTable: "UomUom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMoveLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MoveId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductUomId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantId = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: true),
                    QuantityProductUom = table.Column<int>(type: "integer", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationDestId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMoveLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMoveLine_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMoveLine_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMoveLine_StockLocation_LocationDestId",
                        column: x => x.LocationDestId,
                        principalTable: "StockLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMoveLine_StockLocation_LocationId",
                        column: x => x.LocationId,
                        principalTable: "StockLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMoveLine_StockMove_MoveId",
                        column: x => x.MoveId,
                        principalTable: "StockMove",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMoveLine_StockQuant_QuantId",
                        column: x => x.QuantId,
                        principalTable: "StockQuant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMoveLine_UomUom_ProductUomId",
                        column: x => x.ProductUomId,
                        principalTable: "UomUom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 28, 11, 6, 7, 234, DateTimeKind.Local).AddTicks(8420), new DateTime(2024, 7, 28, 11, 6, 7, 234, DateTimeKind.Local).AddTicks(8420) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 7, 28, 11, 6, 7, 235, DateTimeKind.Local).AddTicks(4660), new DateTime(2024, 7, 28, 11, 6, 7, 235, DateTimeKind.Local).AddTicks(4670) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 7, 28, 11, 6, 7, 235, DateTimeKind.Local).AddTicks(4710), new DateTime(2024, 7, 28, 11, 6, 7, 235, DateTimeKind.Local).AddTicks(4710) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 7, 28, 11, 6, 7, 235, DateTimeKind.Local).AddTicks(4730), new DateTime(2024, 7, 28, 11, 6, 7, 235, DateTimeKind.Local).AddTicks(4730) });

            migrationBuilder.CreateIndex(
                name: "IX_StockLot_CreateUid",
                table: "StockLot",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockLot_WriteUid",
                table: "StockLot",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockMove_CreateUid",
                table: "StockMove",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockMove_LocationDestId",
                table: "StockMove",
                column: "LocationDestId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMove_LocationId",
                table: "StockMove",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMove_PickingId",
                table: "StockMove",
                column: "PickingId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMove_ProductId",
                table: "StockMove",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMove_ProductUomId",
                table: "StockMove",
                column: "ProductUomId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMove_WriteUid",
                table: "StockMove",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockMoveLine_CreateUid",
                table: "StockMoveLine",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockMoveLine_LocationDestId",
                table: "StockMoveLine",
                column: "LocationDestId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMoveLine_LocationId",
                table: "StockMoveLine",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMoveLine_MoveId",
                table: "StockMoveLine",
                column: "MoveId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMoveLine_ProductUomId",
                table: "StockMoveLine",
                column: "ProductUomId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMoveLine_QuantId",
                table: "StockMoveLine",
                column: "QuantId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMoveLine_WriteUid",
                table: "StockMoveLine",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockPicking_CreateUid",
                table: "StockPicking",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockPicking_WriteUid",
                table: "StockPicking",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuant_CreateUid",
                table: "StockQuant",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuant_LocationId",
                table: "StockQuant",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuant_LotId",
                table: "StockQuant",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuant_ProductId",
                table: "StockQuant",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuant_WriteUid",
                table: "StockQuant",
                column: "WriteUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockMoveLine");

            migrationBuilder.DropTable(
                name: "StockMove");

            migrationBuilder.DropTable(
                name: "StockQuant");

            migrationBuilder.DropTable(
                name: "StockPicking");

            migrationBuilder.DropTable(
                name: "StockLot");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(3310), new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(3310) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8640), new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8640) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8690), new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8690) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8700), new DateTime(2024, 7, 25, 14, 11, 54, 27, DateTimeKind.Local).AddTicks(8700) });
        }
    }
}
