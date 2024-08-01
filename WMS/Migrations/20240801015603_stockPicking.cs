using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class stockPicking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMoveLine_StockMove_MoveId",
                table: "StockMoveLine");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMoveLine_StockQuant_QuantId",
                table: "StockMoveLine");

            migrationBuilder.DropForeignKey(
                name: "FK_StockQuant_ProductProduct_ProductId",
                table: "StockQuant");

            migrationBuilder.DropForeignKey(
                name: "FK_StockQuant_StockLocation_LocationId",
                table: "StockQuant");

            migrationBuilder.AddColumn<Guid>(
                name: "BackorderId",
                table: "StockPicking",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeadline",
                table: "StockPicking",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDone",
                table: "StockPicking",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocationDestId",
                table: "StockPicking",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "StockPicking",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StockPicking",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "StockPicking",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "StockPicking",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PickingTypeId",
                table: "StockPicking",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledDate",
                table: "StockPicking",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "StockPicking",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ResPartner",
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
                    table.PrimaryKey("PK_ResPartner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResPartner_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResPartner_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 8, 1, 8, 56, 3, 138, DateTimeKind.Local).AddTicks(7660), new DateTime(2024, 8, 1, 8, 56, 3, 138, DateTimeKind.Local).AddTicks(7670) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6130), new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6140) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6180), new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6180) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6200), new DateTime(2024, 8, 1, 8, 56, 3, 139, DateTimeKind.Local).AddTicks(6200) });

            migrationBuilder.CreateIndex(
                name: "IX_StockPicking_BackorderId",
                table: "StockPicking",
                column: "BackorderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockPicking_LocationDestId",
                table: "StockPicking",
                column: "LocationDestId");

            migrationBuilder.CreateIndex(
                name: "IX_StockPicking_LocationId",
                table: "StockPicking",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockPicking_PartnerId",
                table: "StockPicking",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_StockPicking_PickingTypeId",
                table: "StockPicking",
                column: "PickingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResPartner_CreateUid",
                table: "ResPartner",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ResPartner_WriteUid",
                table: "ResPartner",
                column: "WriteUid");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMoveLine_StockMove_MoveId",
                table: "StockMoveLine",
                column: "MoveId",
                principalTable: "StockMove",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMoveLine_StockQuant_QuantId",
                table: "StockMoveLine",
                column: "QuantId",
                principalTable: "StockQuant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockPicking_ResPartner_PartnerId",
                table: "StockPicking",
                column: "PartnerId",
                principalTable: "ResPartner",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockPicking_StockLocation_LocationDestId",
                table: "StockPicking",
                column: "LocationDestId",
                principalTable: "StockLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockPicking_StockLocation_LocationId",
                table: "StockPicking",
                column: "LocationId",
                principalTable: "StockLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockPicking_StockPickingType_PickingTypeId",
                table: "StockPicking",
                column: "PickingTypeId",
                principalTable: "StockPickingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockPicking_StockPicking_BackorderId",
                table: "StockPicking",
                column: "BackorderId",
                principalTable: "StockPicking",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockQuant_ProductProduct_ProductId",
                table: "StockQuant",
                column: "ProductId",
                principalTable: "ProductProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockQuant_StockLocation_LocationId",
                table: "StockQuant",
                column: "LocationId",
                principalTable: "StockLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMoveLine_StockMove_MoveId",
                table: "StockMoveLine");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMoveLine_StockQuant_QuantId",
                table: "StockMoveLine");

            migrationBuilder.DropForeignKey(
                name: "FK_StockPicking_ResPartner_PartnerId",
                table: "StockPicking");

            migrationBuilder.DropForeignKey(
                name: "FK_StockPicking_StockLocation_LocationDestId",
                table: "StockPicking");

            migrationBuilder.DropForeignKey(
                name: "FK_StockPicking_StockLocation_LocationId",
                table: "StockPicking");

            migrationBuilder.DropForeignKey(
                name: "FK_StockPicking_StockPickingType_PickingTypeId",
                table: "StockPicking");

            migrationBuilder.DropForeignKey(
                name: "FK_StockPicking_StockPicking_BackorderId",
                table: "StockPicking");

            migrationBuilder.DropForeignKey(
                name: "FK_StockQuant_ProductProduct_ProductId",
                table: "StockQuant");

            migrationBuilder.DropForeignKey(
                name: "FK_StockQuant_StockLocation_LocationId",
                table: "StockQuant");

            migrationBuilder.DropTable(
                name: "ResPartner");

            migrationBuilder.DropIndex(
                name: "IX_StockPicking_BackorderId",
                table: "StockPicking");

            migrationBuilder.DropIndex(
                name: "IX_StockPicking_LocationDestId",
                table: "StockPicking");

            migrationBuilder.DropIndex(
                name: "IX_StockPicking_LocationId",
                table: "StockPicking");

            migrationBuilder.DropIndex(
                name: "IX_StockPicking_PartnerId",
                table: "StockPicking");

            migrationBuilder.DropIndex(
                name: "IX_StockPicking_PickingTypeId",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "BackorderId",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "DateDeadline",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "DateDone",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "LocationDestId",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "PickingTypeId",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "ScheduledDate",
                table: "StockPicking");

            migrationBuilder.DropColumn(
                name: "State",
                table: "StockPicking");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 29, 9, 14, 43, 537, DateTimeKind.Local).AddTicks(1450), new DateTime(2024, 7, 29, 9, 14, 43, 537, DateTimeKind.Local).AddTicks(1450) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 7, 29, 9, 14, 43, 537, DateTimeKind.Local).AddTicks(7070), new DateTime(2024, 7, 29, 9, 14, 43, 537, DateTimeKind.Local).AddTicks(7080) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 7, 29, 9, 14, 43, 537, DateTimeKind.Local).AddTicks(7130), new DateTime(2024, 7, 29, 9, 14, 43, 537, DateTimeKind.Local).AddTicks(7130) });

            migrationBuilder.UpdateData(
                table: "StockLocation",
                keyColumn: "Id",
                keyValue: new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc"),
                columns: new[] { "CreateDate", "WriteDate" },
                values: new object[] { new DateTime(2024, 7, 29, 9, 14, 43, 537, DateTimeKind.Local).AddTicks(7150), new DateTime(2024, 7, 29, 9, 14, 43, 537, DateTimeKind.Local).AddTicks(7150) });

            migrationBuilder.AddForeignKey(
                name: "FK_StockMoveLine_StockMove_MoveId",
                table: "StockMoveLine",
                column: "MoveId",
                principalTable: "StockMove",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMoveLine_StockQuant_QuantId",
                table: "StockMoveLine",
                column: "QuantId",
                principalTable: "StockQuant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockQuant_ProductProduct_ProductId",
                table: "StockQuant",
                column: "ProductId",
                principalTable: "ProductProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockQuant_StockLocation_LocationId",
                table: "StockQuant",
                column: "LocationId",
                principalTable: "StockLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
