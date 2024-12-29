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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folders_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedFiles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedFiles", x => new { x.FileId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SharedFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedFiles_Users_UserId",
                        column: x => x.UserId,
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

            migrationBuilder.CreateTable(
                name: "SharedFolders",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FolderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedFolders", x => new { x.FolderId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SharedFolders_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedFolders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password" },
                values: new object[,]
                {
                    { 1, "kimi@gmail.com", "Kimi", "Raikonen", "1234" },
                    { 2, "seb@gmail.com", "Sebastian", "Vettel", "1234" }
                });

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "Content", "CreatedOn", "Extension", "LastChanged", "Name", "OwnerId", "Size" },
                values: new object[,]
                {
                    { 1, "Some random text.", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1403), "txt", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1405), "TodoList", 1, 20L },
                    { 2, "Some random text.", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1409), "txt", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1410), "TodoList2", 1, 20L },
                    { 3, "Some random text.", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1411), "txt", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1411), "TodoList3", 1, 20L },
                    { 4, "Some random text.", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1412), "txt", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1413), "TodoList4", 2, 20L },
                    { 5, "Some random text.", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1414), "txt", new DateTime(2024, 12, 29, 16, 7, 8, 269, DateTimeKind.Utc).AddTicks(1414), "TodoList5", 2, 20L }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "IsRoot", "Name", "OwnerId", "ParentFolderId" },
                values: new object[,]
                {
                    { 1, true, "root", 1, null },
                    { 4, true, "root", 2, null }
                });

            migrationBuilder.InsertData(
                table: "FolderFiles",
                columns: new[] { "FileId", "FolderId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "IsRoot", "Name", "OwnerId", "ParentFolderId" },
                values: new object[,]
                {
                    { 2, false, "obj", 1, 1 },
                    { 3, false, "bin", 1, 1 },
                    { 5, false, "new", 2, 4 },
                    { 6, false, "some", 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "SharedFiles",
                columns: new[] { "FileId", "UserId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "FolderFiles",
                columns: new[] { "FileId", "FolderId" },
                values: new object[,]
                {
                    { 2, 2 },
                    { 3, 3 },
                    { 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "SharedFolders",
                columns: new[] { "FolderId", "UserId" },
                values: new object[,]
                {
                    { 2, 2 },
                    { 5, 1 }
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

            migrationBuilder.CreateIndex(
                name: "IX_SharedFiles_UserId",
                table: "SharedFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedFolders_UserId",
                table: "SharedFolders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderFiles");

            migrationBuilder.DropTable(
                name: "SharedFiles");

            migrationBuilder.DropTable(
                name: "SharedFolders");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
