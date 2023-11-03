using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "InboundProduct");

            migrationBuilder.DropTable(
                name: "LocationOutboundProduct");

            migrationBuilder.DropTable(
                name: "OutboundProduct");

            migrationBuilder.DropIndex(
                name: "IX_Product_SupplierId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "InventoryCount",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "ReceiptNumber",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "ReceiptType",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "ReceivedDate",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "InventoryCount",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ReceivedDate",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Receipt",
                newName: "PurchaseUnitPrice");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Receipt",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Receipt",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Receipt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PickingRequest",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "PickingRequest",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PickingRequest",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Inventory",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReceiptInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptId = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptInventory_Inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptInventory_Receipt_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "Receipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_ProductId",
                table: "Receipt",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingRequest_ProductId",
                table: "PickingRequest",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptInventory_InventoryId",
                table: "ReceiptInventory",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptInventory_ReceiptId",
                table: "ReceiptInventory",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_PickingRequest_Product_ProductId",
                table: "PickingRequest",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipt_Product_ProductId",
                table: "Receipt",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PickingRequest_Product_ProductId",
                table: "PickingRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipt_Product_ProductId",
                table: "Receipt");

            migrationBuilder.DropTable(
                name: "ReceiptInventory");

            migrationBuilder.DropIndex(
                name: "IX_Receipt_ProductId",
                table: "Receipt");

            migrationBuilder.DropIndex(
                name: "IX_PickingRequest_ProductId",
                table: "PickingRequest");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PickingRequest");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PickingRequest");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "PurchaseUnitPrice",
                table: "Receipt",
                newName: "TotalAmount");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Receipt",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "InventoryCount",
                table: "Receipt",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptNumber",
                table: "Receipt",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptType",
                table: "Receipt",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedDate",
                table: "Receipt",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryCount",
                table: "Product",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Product",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "Product",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PickingRequest",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedDate",
                table: "Inventory",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Inventory",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InboundProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchNumber = table.Column<int>(type: "integer", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ManufacturedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    PurchaseUnitPrice = table.Column<double>(type: "double precision", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TotalCost = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InboundProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InboundProduct_Receipt_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "Receipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutboundProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PickingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboundProduct_PickingRequest_PickingId",
                        column: x => x.PickingId,
                        principalTable: "PickingRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutboundProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationOutboundProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OutboundProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationOutboundProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationOutboundProduct_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationOutboundProduct_OutboundProduct_OutboundProductId",
                        column: x => x.OutboundProductId,
                        principalTable: "OutboundProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_SupplierId",
                table: "Product",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InboundProduct_ProductId",
                table: "InboundProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InboundProduct_ReceiptId",
                table: "InboundProduct",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationOutboundProduct_LocationId",
                table: "LocationOutboundProduct",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationOutboundProduct_OutboundProductId",
                table: "LocationOutboundProduct",
                column: "OutboundProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundProduct_PickingId",
                table: "OutboundProduct",
                column: "PickingId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundProduct_ProductId",
                table: "OutboundProduct",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
