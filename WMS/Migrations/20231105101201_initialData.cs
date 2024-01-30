using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class initialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "IsDeactive", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"), null, "Role for Admin", false, "Admin", "ADMIN" },
                    { new Guid("7119a2e7-e680-4ecd-8344-0c53082cdc87"), null, "Role for Manager", false, "Manager", "MANAGER" },
                    { new Guid("931c6340-f21a-4bbf-b71c-e39d7cebc997"), null, "Role for Staff", false, "Staff", "STAFF" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CurrenNoticeCount", "DateCreated", "DateUpdated", "Email", "EmailConfirmed", "FcmToken", "FirstName", "IsActive", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserAva", "UserName" },
                values: new object[] { new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"), 0, null, "8a12fe29-7fe8-41f3-9af2-54b1ca8d4207", 0, new DateTime(2023, 11, 5, 17, 12, 1, 896, DateTimeKind.Local).AddTicks(2280), new DateTime(2023, 11, 5, 17, 12, 1, 896, DateTimeKind.Local).AddTicks(2280), "lieudvq0302@gmail.com", false, null, "System", true, false, "Admin", true, null, "LIEUDVQ@0302@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEGAV0YwJmXtNmREj0oD2OX9feN5dC0WIPykTsTLuLhOCpJUPZSBQQww0K/IJ4v7NRw==", "012345609", false, "SMERHXAHGAGZWB7SKS2FZAH3PHQC7WXL", false, null, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"), new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7119a2e7-e680-4ecd-8344-0c53082cdc87"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("931c6340-f21a-4bbf-b71c-e39d7cebc997"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"), new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"));
        }
    }
}
