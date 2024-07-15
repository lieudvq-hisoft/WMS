using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductVariantCombination",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductTemplateAttributeValueId = table.Column<Guid>(type: "uuid", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: true),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantCombination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantCombination_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductVariantCombination_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductVariantCombination_ProductProduct_ProductProductId",
                        column: x => x.ProductProductId,
                        principalTable: "ProductProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantCombination_ProductTemplateAttributeValue_Pro~",
                        column: x => x.ProductTemplateAttributeValueId,
                        principalTable: "ProductTemplateAttributeValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 15, 14, 34, 28, 706, DateTimeKind.Local).AddTicks(6190), new DateTime(2024, 7, 15, 14, 34, 28, 706, DateTimeKind.Local).AddTicks(6190) });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantCombination_CreateUid",
                table: "ProductVariantCombination",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantCombination_ProductProductId",
                table: "ProductVariantCombination",
                column: "ProductProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantCombination_ProductTemplateAttributeValueId",
                table: "ProductVariantCombination",
                column: "ProductTemplateAttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantCombination_WriteUid",
                table: "ProductVariantCombination",
                column: "WriteUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariantCombination");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2024, 7, 15, 14, 28, 11, 84, DateTimeKind.Local).AddTicks(4700), new DateTime(2024, 7, 15, 14, 28, 11, 84, DateTimeKind.Local).AddTicks(4700) });
        }
    }
}
