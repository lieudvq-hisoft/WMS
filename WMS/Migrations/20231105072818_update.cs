using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RackLevelId",
                table: "Location",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "SectionNumber",
                table: "Location",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rack",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RackNumber = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TotalLevel = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rack", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RackLevel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RackId = table.Column<Guid>(type: "uuid", nullable: false),
                    LevelNumber = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RackLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RackLevel_Rack_RackId",
                        column: x => x.RackId,
                        principalTable: "Rack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Location_RackLevelId",
                table: "Location",
                column: "RackLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_RackLevel_RackId",
                table: "RackLevel",
                column: "RackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_RackLevel_RackLevelId",
                table: "Location",
                column: "RackLevelId",
                principalTable: "RackLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_RackLevel_RackLevelId",
                table: "Location");

            migrationBuilder.DropTable(
                name: "RackLevel");

            migrationBuilder.DropTable(
                name: "Rack");

            migrationBuilder.DropIndex(
                name: "IX_Location_RackLevelId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "RackLevelId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "SectionNumber",
                table: "Location");
        }
    }
}
