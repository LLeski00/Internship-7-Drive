using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Drive.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    ParentFolderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Files_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
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

            migrationBuilder.CreateTable(
                name: "FileComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    FileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileComments_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileComments_Users_AuthorId",
                        column: x => x.AuthorId,
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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password" },
                values: new object[,]
                {
                    { 1, "kimi@gmail.com", "Kimi", "Raikkonen", "1234" },
                    { 2, "seb@gmail.com", "Sebastian", "Vettel", "1234" },
                    { 3, "lewis@gmail.com", "Lewis", "Hamilton", "1234" },
                    { 4, "max@gmail.com", "Max", "Verstappen", "1234" },
                    { 5, "charles@gmail.com", "Charles", "Leclerc", "1234" },
                    { 6, "lando@gmail.com", "Lando", "Norris", "1234" },
                    { 7, "fernando@gmail.com", "Fernando", "Alonso", "1234" },
                    { 8, "george@gmail.com", "George", "Russell", "1234" },
                    { 9, "carlos@gmail.com", "Carlos", "Sainz", "1234" },
                    { 10, "daniel@gmail.com", "Daniel", "Ricciardo", "1234" }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Name", "OwnerId", "ParentFolderId" },
                values: new object[,]
                {
                    { 1, "root", 1, null },
                    { 2, "root", 2, null },
                    { 3, "root", 3, null },
                    { 4, "root", 4, null },
                    { 5, "root", 5, null },
                    { 6, "root", 6, null },
                    { 7, "root", 7, null },
                    { 8, "root", 8, null },
                    { 9, "root", 9, null },
                    { 10, "root", 10, null }
                });

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "Content", "CreatedOn", "Extension", "LastChanged", "Name", "OwnerId", "ParentFolderId", "Size" },
                values: new object[,]
                {
                    { 1, "Some random text.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9205), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9206), "TodoList", 1, 1, 20L },
                    { 2, "Some project planning text.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9209), "docx", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9209), "ProjectPlan", 1, 1, 40L },
                    { 6, "Design document draft.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9215), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9216), "DesignDoc", 2, 2, 25L },
                    { 7, "Project financial plan.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9217), "xlsx", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9217), "ProjectPlan", 2, 2, 35L },
                    { 12, "Agenda for the meeting.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9223), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9223), "MeetingAgenda", 3, 3, 25L },
                    { 13, "Project status report.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9224), "docx", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9224), "StatusReport", 3, 3, 30L },
                    { 16, "Race results from 2023 season.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9228), "csv", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9228), "RaceResults", 4, 4, 40L },
                    { 17, "Driver performance stats.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9229), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9229), "DriverStats", 4, 4, 30L },
                    { 21, "Track 1 from the album.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9234), "mp3", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9235), "MusicTrack1", 5, 5, 40L },
                    { 22, "Playlist of favorite tracks.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9238), "m3u", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9238), "Playlist1", 5, 5, 10L },
                    { 25, "Game installation file.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9241), "exe", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9242), "GameInstall", 6, 6, 200L },
                    { 26, "Zip file of old projects.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9243), "zip", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9243), "CompressedFiles", 6, 6, 300L },
                    { 29, "Save game file.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9246), "sav", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9247), "GameSave1", 7, 7, 100L },
                    { 30, "Save game file 2.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9248), "sav", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9248), "GameSave2", 7, 7, 120L },
                    { 33, "Lap times for 2023 season.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9251), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9252), "LapTimes2023", 8, 8, 20L },
                    { 34, "Telemetry data for season 2023.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9253), "csv", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9253), "TelemetryData", 8, 8, 40L },
                    { 37, "Random notes for project.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9256), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9256), "Notes1", 9, 9, 20L },
                    { 38, "Ideas for the new project.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9258), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9258), "Ideas", 9, 9, 25L },
                    { 41, "Log file 1.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9261), "log", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9261), "LogFile1", 10, 10, 10L },
                    { 42, "Cache data file.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9262), "dat", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9262), "CacheFile", 10, 10, 50L }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Name", "OwnerId", "ParentFolderId" },
                values: new object[,]
                {
                    { 11, "Documents", 1, 1 },
                    { 12, "Images", 1, 1 },
                    { 16, "Work", 2, 2 },
                    { 17, "Personal", 2, 2 },
                    { 21, "Projects", 3, 3 },
                    { 22, "Backups", 3, 3 },
                    { 26, "RacingData", 4, 4 },
                    { 27, "Photos", 4, 4 },
                    { 31, "Videos", 5, 5 },
                    { 32, "Music", 5, 5 },
                    { 36, "Downloads", 6, 6 },
                    { 37, "Archives", 6, 6 },
                    { 41, "Games", 7, 7 },
                    { 42, "Settings", 7, 7 },
                    { 46, "Simulations", 8, 8 },
                    { 47, "Telemetry", 8, 8 },
                    { 51, "Notes", 9, 9 },
                    { 52, "Drafts", 9, 9 },
                    { 56, "Logs", 10, 10 },
                    { 57, "Temp", 10, 10 }
                });

            migrationBuilder.InsertData(
                table: "FileComments",
                columns: new[] { "Id", "AuthorId", "Content", "CreatedOn", "FileId", "LastChanged" },
                values: new object[,]
                {
                    { 1, 1, "Some random comment", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9373), 1, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9374) },
                    { 2, 2, "Even more random comment", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9378), 1, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9378) },
                    { 3, 3, "This is another comment", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9379), 2, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9379) },
                    { 7, 7, "Helpful document", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9383), 6, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9384) },
                    { 8, 8, "Awesome project plan", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9384), 7, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9385) },
                    { 13, 3, "Noticed a typo", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9389), 12, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9389) },
                    { 14, 4, "I agree with this analysis", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9390), 13, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9390) },
                    { 17, 7, "Just saved this!", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9393), 16, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9393) },
                    { 18, 8, "Great game save!", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9394), 17, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9394) },
                    { 22, 2, "Awesome backup!", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9398), 21, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9398) },
                    { 23, 3, "Very useful file", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9398), 22, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9399) },
                    { 26, 6, "Great analysis", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9401), 25, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9401) },
                    { 27, 7, "Perfect telemetry data", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9402), 26, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9402) },
                    { 30, 10, "Very useful compressed files", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9405), 29, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9405) },
                    { 31, 1, "Nice install files", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9406), 30, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9406) },
                    { 34, 4, "Enjoying the music", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9409), 33, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9409) },
                    { 35, 5, "Great concert footage", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9410), 34, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9410) },
                    { 38, 8, "Love the car setups", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9412), 37, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9413) },
                    { 39, 9, "Interesting ideas", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9413), 38, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9414) },
                    { 42, 2, "Great for new ideas", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9416), 41, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9416) },
                    { 43, 3, "Useful error logs", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9417), 42, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9417) }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Name", "OwnerId", "ParentFolderId" },
                values: new object[,]
                {
                    { 13, "Work", 1, 11 },
                    { 14, "Projects", 1, 11 },
                    { 15, "Screenshots", 1, 12 },
                    { 18, "Reports", 2, 16 },
                    { 19, "Drafts", 2, 17 },
                    { 20, "Archives", 2, 17 },
                    { 23, "Code", 3, 21 },
                    { 24, "Logs", 3, 22 },
                    { 25, "OldProjects", 3, 21 },
                    { 28, "Season2023", 4, 26 },
                    { 29, "Season2022", 4, 26 },
                    { 30, "PodiumPhotos", 4, 27 },
                    { 33, "Clips", 5, 31 },
                    { 34, "Playlists", 5, 32 },
                    { 35, "Concerts", 5, 32 },
                    { 38, "Compressed", 6, 36 },
                    { 39, "Installers", 6, 36 },
                    { 40, "OldArchives", 6, 37 },
                    { 43, "SaveFiles", 7, 41 },
                    { 44, "Mods", 7, 41 },
                    { 45, "Config", 7, 42 },
                    { 48, "LapTimes", 8, 46 },
                    { 49, "Analytics", 8, 47 },
                    { 50, "CarSetups", 8, 46 },
                    { 53, "Ideas", 9, 51 },
                    { 54, "Plans", 9, 51 },
                    { 55, "Templates", 9, 52 },
                    { 58, "ErrorLogs", 10, 56 },
                    { 59, "Cache", 10, 57 },
                    { 60, "SessionLogs", 10, 56 }
                });

            migrationBuilder.InsertData(
                table: "SharedFolders",
                columns: new[] { "FolderId", "UserId" },
                values: new object[,]
                {
                    { 11, 2 },
                    { 12, 2 },
                    { 16, 1 },
                    { 17, 1 },
                    { 21, 1 },
                    { 21, 2 },
                    { 22, 1 },
                    { 22, 2 }
                });

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "Content", "CreatedOn", "Extension", "LastChanged", "Name", "OwnerId", "ParentFolderId", "Size" },
                values: new object[,]
                {
                    { 3, "Work proposal document.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9211), "pdf", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9211), "WorkProposal", 1, 13, 50L },
                    { 4, "Task list for the week.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9212), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9213), "TaskList", 1, 14, 30L },
                    { 5, "Profile picture.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9214), "jpg", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9214), "ProfilePicture", 1, 15, 15L },
                    { 8, "Meeting notes from last session.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9218), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9218), "MeetingNotes", 2, 18, 20L },
                    { 9, "Financial report for Q1.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9219), "pdf", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9219), "Report", 2, 18, 40L },
                    { 10, "List of gift ideas for birthday.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9220), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9221), "BirthdayGiftIdeas", 2, 19, 15L },
                    { 11, "Vacation plan spreadsheet.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9222), "xlsx", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9222), "VacationPlan", 2, 20, 50L },
                    { 14, "Compressed source code.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9225), "zip", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9226), "CodeFiles", 3, 23, 100L },
                    { 15, "Server log files.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9226), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9227), "ServerLogs", 3, 24, 150L },
                    { 18, "Detailed results for 2023.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9230), "xlsx", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9231), "Season2023Results", 4, 28, 80L },
                    { 19, "Detailed results for 2022.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9232), "xlsx", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9232), "Season2022Results", 4, 29, 75L },
                    { 20, "Podium photo from the 2023 season.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9233), "jpg", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9233), "PodiumPhoto1", 4, 30, 20L },
                    { 23, "Official music video.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9239), "mp4", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9239), "MusicVideo1", 5, 33, 100L },
                    { 24, "Concert footage.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9240), "mp4", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9241), "Concert1", 5, 35, 250L },
                    { 27, "Software installer package.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9244), "exe", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9244), "SoftwareInstaller", 6, 38, 150L },
                    { 28, "Old compressed archives.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9245), "zip", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9245), "OldArchives", 6, 40, 500L },
                    { 31, "Mods for the game.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9249), "zip", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9249), "GameMods", 7, 43, 50L },
                    { 32, "Configuration file for the game.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9250), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9250), "ConfigFile", 7, 45, 15L },
                    { 35, "Lap time analysis for 2023.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9254), "xlsx", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9254), "LapTimeAnalysis", 8, 49, 60L },
                    { 36, "Car setups for season 2023.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9255), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9255), "CarSetups2023", 8, 50, 50L },
                    { 39, "Draft of a new idea.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9259), "txt", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9259), "IdeaDraft", 9, 53, 30L },
                    { 40, "Draft of project plan.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9260), "docx", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9260), "ProjectPlanDraft", 9, 54, 40L },
                    { 43, "Error log for system.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9263), "log", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9264), "ErrorLog1", 10, 58, 30L },
                    { 44, "Session log file.", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9264), "log", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9265), "SessionLog1", 10, 60, 60L }
                });

            migrationBuilder.InsertData(
                table: "SharedFolders",
                columns: new[] { "FolderId", "UserId" },
                values: new object[,]
                {
                    { 13, 2 },
                    { 14, 2 },
                    { 15, 2 },
                    { 18, 1 },
                    { 19, 1 },
                    { 20, 1 },
                    { 23, 1 },
                    { 23, 2 },
                    { 24, 1 },
                    { 24, 2 },
                    { 25, 1 },
                    { 25, 2 }
                });

            migrationBuilder.InsertData(
                table: "FileComments",
                columns: new[] { "Id", "AuthorId", "Content", "CreatedOn", "FileId", "LastChanged" },
                values: new object[,]
                {
                    { 4, 4, "A detailed comment", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9380), 3, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9380) },
                    { 5, 5, "Comment on the file", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9381), 4, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9381) },
                    { 6, 6, "Great file!", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9382), 5, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9383) },
                    { 9, 9, "Interesting design doc", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9385), 8, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9385) },
                    { 10, 10, "I like this idea", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9386), 9, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9387) },
                    { 11, 1, "Great report", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9387), 10, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9388) },
                    { 12, 2, "Very detailed race results", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9388), 11, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9388) },
                    { 15, 5, "This is a useful document", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9391), 14, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9391) },
                    { 16, 6, "Needs revision", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9392), 15, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9392) },
                    { 19, 9, "Good configuration", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9395), 18, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9395) },
                    { 20, 10, "Loving these game mods", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9396), 19, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9396) },
                    { 21, 1, "Can’t wait to try this", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9397), 20, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9397) },
                    { 24, 4, "Nice code files", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9399), 23, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9400) },
                    { 25, 5, "Logs are clean", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9400), 24, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9401) },
                    { 28, 8, "Season 2022 results are helpful", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9403), 27, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9403) },
                    { 29, 9, "Old project plans", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9404), 28, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9404) },
                    { 32, 2, "Game data is great", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9406), 31, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9407) },
                    { 33, 3, "This game setup is useful", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9407), 32, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9408) },
                    { 36, 6, "Fantastic lap times", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9411), 35, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9411) },
                    { 37, 7, "Really insightful lap time analysis", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9412), 36, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9412) },
                    { 40, 10, "Looking forward to this draft", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9414), 39, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9415) },
                    { 41, 1, "Very helpful file", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9415), 40, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9415) },
                    { 44, 4, "Session logs look good", new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9418), 43, new DateTime(2025, 1, 3, 20, 34, 50, 493, DateTimeKind.Utc).AddTicks(9418) }
                });

            migrationBuilder.InsertData(
                table: "SharedFiles",
                columns: new[] { "FileId", "UserId" },
                values: new object[,]
                {
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 2 },
                    { 8, 1 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 1 },
                    { 14, 1 },
                    { 14, 2 },
                    { 15, 1 },
                    { 15, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileComments_AuthorId",
                table: "FileComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_FileComments_FileId",
                table: "FileComments",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_OwnerId",
                table: "Files",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ParentFolderId",
                table: "Files",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_OwnerId_ParentFolderId",
                table: "Folders",
                columns: new[] { "OwnerId", "ParentFolderId" },
                unique: true,
                filter: "\"ParentFolderId\" IS NULL");

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
                name: "FileComments");

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
