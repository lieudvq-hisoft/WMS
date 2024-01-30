using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateReceipt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipt_AspNetUsers_CreatedBy",
                table: "Receipt");

            migrationBuilder.DropIndex(
                name: "IX_Receipt_CreatedBy",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Receipt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Receipt",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_CreatedBy",
                table: "Receipt",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipt_AspNetUsers_CreatedBy",
                table: "Receipt",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
