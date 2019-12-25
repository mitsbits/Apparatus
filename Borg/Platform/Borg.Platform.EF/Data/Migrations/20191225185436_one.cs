using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Platform.EF.Data.Migrations
{
    public partial class one : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "DictionaryState_Id_seq");

            migrationBuilder.CreateSequence<int>(
                name: "Language_Id_seq");

            migrationBuilder.CreateSequence<int>(
                name: "Menu_Id_seq");

            migrationBuilder.CreateSequence<int>(
                name: "MenuItem_Id_seq");

            migrationBuilder.CreateSequence<int>(
                name: "Page_Id_seq");

            migrationBuilder.CreateSequence<int>(
                name: "Tenant_Id_seq");

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR Language_Id_seq"),
                    TwoLetterISOCode = table.Column<string>(unicode: false, maxLength: 2, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR Tenant_Id_seq"),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(maxLength: 100, nullable: true, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Depth = table.Column<int>(nullable: false, defaultValue: 0),
                    Hierarchy = table.Column<string>(unicode: false, maxLength: 512, nullable: false, defaultValue: ""),
                    Key = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    Value = table.Column<string>(maxLength: 2147483647, nullable: false, defaultValue: ""),
                    Descriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryState", x => new { x.Id, x.TenantId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_DictionaryState_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DictionaryState_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Depth = table.Column<int>(nullable: false),
                    Hierarchy = table.Column<string>(nullable: false),
                    EntryType = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FolderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entry_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entry_Entry_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Entry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entry_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entry_Entry_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Entry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(nullable: true),
                    ActivationID = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValue: ""),
                    DeActivationID = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValue: ""),
                    IsActive = table.Column<bool>(nullable: false),
                    IsCurrentlyActive = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => new { x.Id, x.TenantId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_Menus_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Menus_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(nullable: true),
                    ActivationID = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValue: ""),
                    DeActivationID = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValue: ""),
                    IsActive = table.Column<bool>(nullable: false),
                    IsCurrentlyActive = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Depth = table.Column<int>(nullable: false, defaultValue: 0),
                    Hierarchy = table.Column<string>(unicode: false, maxLength: 512, nullable: false, defaultValue: ""),
                    Slug = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    FullSlug = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    Title = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => new { x.Id, x.TenantId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_Pages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pages_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(nullable: true),
                    ActivationID = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValue: ""),
                    DeActivationID = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValue: ""),
                    IsActive = table.Column<bool>(nullable: false),
                    IsCurrentlyActive = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Depth = table.Column<int>(nullable: false, defaultValue: 0),
                    Hierarchy = table.Column<string>(unicode: false, maxLength: 512, nullable: false, defaultValue: ""),
                    MenuId = table.Column<int>(nullable: false),
                    Targets = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => new { x.Id, x.TenantId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_MenuItem_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItem_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItem_Menus_TenantId_LanguageId_MenuId",
                        columns: x => new { x.TenantId, x.LanguageId, x.MenuId },
                        principalTable: "Menus",
                        principalColumns: new[] { "Id", "TenantId", "LanguageId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryState_LanguageId",
                table: "DictionaryState",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryState_TenantId",
                table: "DictionaryState",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_LanguageId",
                table: "Entry",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_ParentId",
                table: "Entry",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_TenantId",
                table: "Entry",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_FolderId",
                table: "Entry",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_LanguageId",
                table: "MenuItem",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "FK_Menu_Id",
                table: "MenuItem",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_TenantId_LanguageId_MenuId",
                table: "MenuItem",
                columns: new[] { "TenantId", "LanguageId", "MenuId" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_LanguageId",
                table: "Menus",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_TenantId",
                table: "Menus",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_LanguageId",
                table: "Pages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_TenantId",
                table: "Pages",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictionaryState");

            migrationBuilder.DropTable(
                name: "Entry");

            migrationBuilder.DropTable(
                name: "MenuItem");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropSequence(
                name: "DictionaryState_Id_seq");

            migrationBuilder.DropSequence(
                name: "Language_Id_seq");

            migrationBuilder.DropSequence(
                name: "Menu_Id_seq");

            migrationBuilder.DropSequence(
                name: "MenuItem_Id_seq");

            migrationBuilder.DropSequence(
                name: "Page_Id_seq");

            migrationBuilder.DropSequence(
                name: "Tenant_Id_seq");
        }
    }
}
