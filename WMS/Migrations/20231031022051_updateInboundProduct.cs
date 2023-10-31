using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateInboundProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InboundProduct_Receipt_ReceivedId",
                table: "InboundProduct");

            migrationBuilder.RenameColumn(
                name: "ReceivedId",
                table: "InboundProduct",
                newName: "ReceiptId");

            migrationBuilder.RenameIndex(
                name: "IX_InboundProduct_ReceivedId",
                table: "InboundProduct",
                newName: "IX_InboundProduct_ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_InboundProduct_Receipt_ReceiptId",
                table: "InboundProduct",
                column: "ReceiptId",
                principalTable: "Receipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InboundProduct_Receipt_ReceiptId",
                table: "InboundProduct");

            migrationBuilder.RenameColumn(
                name: "ReceiptId",
                table: "InboundProduct",
                newName: "ReceivedId");

            migrationBuilder.RenameIndex(
                name: "IX_InboundProduct_ReceiptId",
                table: "InboundProduct",
                newName: "IX_InboundProduct_ReceivedId");

            migrationBuilder.AddForeignKey(
                name: "FK_InboundProduct_Receipt_ReceivedId",
                table: "InboundProduct",
                column: "ReceivedId",
                principalTable: "Receipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
