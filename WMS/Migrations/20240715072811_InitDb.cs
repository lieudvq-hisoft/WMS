using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDeactive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    UserAva = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CurrenNoticeCount = table.Column<int>(type: "integer", nullable: false),
                    FcmToken = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttribute_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductAttribute_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductRemoval",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRemoval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRemoval_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductRemoval_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UomCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UomCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UomCategory_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UomCategory_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValue_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductAttributeValue_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductAttributeValue_ProductAttribute_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "ProductAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CompleteName = table.Column<string>(type: "text", nullable: true),
                    ParentPath = table.Column<string>(type: "text", nullable: true),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    RemovalStrategyId = table.Column<Guid>(type: "uuid", nullable: false),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategory_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCategory_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCategory_ProductCategory_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProductCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCategory_ProductRemoval_RemovalStrategyId",
                        column: x => x.RemovalStrategyId,
                        principalTable: "ProductRemoval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UomUom",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    UomType = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Factor = table.Column<decimal>(type: "numeric", nullable: false),
                    Rounding = table.Column<decimal>(type: "numeric", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UomUom", x => x.Id);
                    table.CheckConstraint("uom_uom_factor_gt_zero", "\"Factor\" <> 0");
                    table.CheckConstraint("uom_uom_factor_reference_is_one", "((\"UomType\" = 'reference' AND \"Factor\" = 1.0) OR (\"UomType\" <> 'reference'))");
                    table.CheckConstraint("uom_uom_rounding_gt_zero", "\"Rounding\" > 0");
                    table.ForeignKey(
                        name: "FK_UomUom_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UomUom_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UomUom_UomCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "UomCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DetailedType = table.Column<string>(type: "text", nullable: false),
                    Tracking = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: true),
                    UomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTemplate_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductTemplate_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductTemplate_ProductCategory_CategId",
                        column: x => x.CategId,
                        principalTable: "ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTemplate_UomUom_UomId",
                        column: x => x.UomId,
                        principalTable: "UomUom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductTmplId = table.Column<Guid>(type: "uuid", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: true),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductProduct_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductProduct_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductProduct_ProductTemplate_ProductTmplId",
                        column: x => x.ProductTmplId,
                        principalTable: "ProductTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductTemplateAttributeLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductTmplId = table.Column<Guid>(type: "uuid", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: true),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTemplateAttributeLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTemplateAttributeLine_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductTemplateAttributeLine_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductTemplateAttributeLine_ProductAttribute_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "ProductAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductTemplateAttributeLine_ProductTemplate_ProductTmplId",
                        column: x => x.ProductTmplId,
                        principalTable: "ProductTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductTemplateAttributeValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductAttributeValueId = table.Column<Guid>(type: "uuid", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: true),
                    CreateUid = table.Column<Guid>(type: "uuid", nullable: true),
                    WriteUid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WriteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTemplateAttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTemplateAttributeValue_AspNetUsers_CreateUid",
                        column: x => x.CreateUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductTemplateAttributeValue_AspNetUsers_WriteUid",
                        column: x => x.WriteUid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductTemplateAttributeValue_ProductAttributeValue_Product~",
                        column: x => x.ProductAttributeValueId,
                        principalTable: "ProductAttributeValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductTemplateAttributeValue_ProductTemplateAttributeLine_~",
                        column: x => x.AttributeLineId,
                        principalTable: "ProductTemplateAttributeLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                values: new object[] { new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"), 0, null, "8a12fe29-7fe8-41f3-9af2-54b1ca8d4207", 0, new DateTime(2024, 7, 15, 14, 28, 11, 84, DateTimeKind.Local).AddTicks(4700), new DateTime(2024, 7, 15, 14, 28, 11, 84, DateTimeKind.Local).AddTicks(4700), "lieudvq0302@gmail.com", false, null, "System", true, false, "Admin", true, null, "LIEUDVQ0302@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEGAV0YwJmXtNmREj0oD2OX9feN5dC0WIPykTsTLuLhOCpJUPZSBQQww0K/IJ4v7NRw==", "012345609", false, "SMERHXAHGAGZWB7SKS2FZAH3PHQC7WXL", false, null, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("003f7676-1d91-4143-9bfd-7a6c17c156fe"), new Guid("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b") });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_CreateUid",
                table: "ProductAttribute",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_WriteUid",
                table: "ProductAttribute",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValue_AttributeId",
                table: "ProductAttributeValue",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValue_CreateUid",
                table: "ProductAttributeValue",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValue_WriteUid",
                table: "ProductAttributeValue",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_CreateUid",
                table: "ProductCategory",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ParentId",
                table: "ProductCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_RemovalStrategyId",
                table: "ProductCategory",
                column: "RemovalStrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_WriteUid",
                table: "ProductCategory",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProduct_CreateUid",
                table: "ProductProduct",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProduct_ProductTmplId",
                table: "ProductProduct",
                column: "ProductTmplId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProduct_WriteUid",
                table: "ProductProduct",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRemoval_CreateUid",
                table: "ProductRemoval",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRemoval_WriteUid",
                table: "ProductRemoval",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplate_CategId",
                table: "ProductTemplate",
                column: "CategId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplate_CreateUid",
                table: "ProductTemplate",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplate_UomId",
                table: "ProductTemplate",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplate_WriteUid",
                table: "ProductTemplate",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplateAttributeLine_AttributeId",
                table: "ProductTemplateAttributeLine",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplateAttributeLine_CreateUid",
                table: "ProductTemplateAttributeLine",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplateAttributeLine_ProductTmplId",
                table: "ProductTemplateAttributeLine",
                column: "ProductTmplId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplateAttributeLine_WriteUid",
                table: "ProductTemplateAttributeLine",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplateAttributeValue_AttributeLineId",
                table: "ProductTemplateAttributeValue",
                column: "AttributeLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplateAttributeValue_CreateUid",
                table: "ProductTemplateAttributeValue",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplateAttributeValue_ProductAttributeValueId",
                table: "ProductTemplateAttributeValue",
                column: "ProductAttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTemplateAttributeValue_WriteUid",
                table: "ProductTemplateAttributeValue",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_UomCategory_CreateUid",
                table: "UomCategory",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_UomCategory_WriteUid",
                table: "UomCategory",
                column: "WriteUid");

            migrationBuilder.CreateIndex(
                name: "IX_UomUom_CategoryId",
                table: "UomUom",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UomUom_CreateUid",
                table: "UomUom",
                column: "CreateUid");

            migrationBuilder.CreateIndex(
                name: "IX_UomUom_WriteUid",
                table: "UomUom",
                column: "WriteUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ProductProduct");

            migrationBuilder.DropTable(
                name: "ProductTemplateAttributeValue");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ProductAttributeValue");

            migrationBuilder.DropTable(
                name: "ProductTemplateAttributeLine");

            migrationBuilder.DropTable(
                name: "ProductAttribute");

            migrationBuilder.DropTable(
                name: "ProductTemplate");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "UomUom");

            migrationBuilder.DropTable(
                name: "ProductRemoval");

            migrationBuilder.DropTable(
                name: "UomCategory");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
