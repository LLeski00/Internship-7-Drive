using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Drive.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Extension = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsRoot = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    ParentFolderId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Folders_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FolderFiles",
                columns: table => new
                {
                    FolderId = table.Column<int>(type: "integer", nullable: false),
                    FileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderFiles", x => new { x.FolderId, x.FileId });
                    table.ForeignKey(
                        name: "FK_FolderFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderFiles_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password" },
                values: new object[] { 1, "llesko00@gmail.com", "Luka", "Leskovec", "1234" });

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "Content", "CreatedOn", "Extension", "LastChanged", "Name", "OwnerId", "Size" },
                values: new object[,]
                {
                    { 1, "Some random text.", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(237), "txt", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(238), "TodoList", 1, 20L },
                    { 2, "Some random text.", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(241), "txt", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(242), "TodoList2", 1, 20L },
                    { 3, "Some random text.", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(243), "txt", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(244), "TodoList3", 1, 20L },
                    { 4, "Some random text.", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(245), "txt", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(245), "TodoList4", 1, 20L },
                    { 5, "Some random text.", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(246), "txt", new DateTime(2024, 12, 24, 14, 21, 1, 362, DateTimeKind.Utc).AddTicks(246), "TodoList5", 1, 20L }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "IsRoot", "Name", "OwnerId", "ParentFolderId" },
                values: new object[] { 1, true, "root", 1, null });

            migrationBuilder.InsertData(
                table: "FolderFiles",
                columns: new[] { "FileId", "FolderId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 4, 1 },
                    { 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "IsRoot", "Name", "OwnerId", "ParentFolderId" },
                values: new object[,]
                {
                    { 2, false, "obj", 1, 1 },
                    { 3, false, "bin", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "FolderFiles",
                columns: new[] { "FileId", "FolderId" },
                values: new object[,]
                {
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_OwnerId",
                table: "Files",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderFiles_FileId",
                table: "FolderFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_OwnerId_IsRoot",
                table: "Folders",
                columns: new[] { "OwnerId", "IsRoot" },
                unique: true,
                filter: "\"IsRoot\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderFiles");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
