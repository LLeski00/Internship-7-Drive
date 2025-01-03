using Drive.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Data.Seeds;

public static class DatabaseSeeder
{
    public static void Seed(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasData(new List<User>
            {
                new User("kimi@gmail.com", "1234", "Kimi", "Raikkonen")
                {
                    Id = 1,
                },
                new User("seb@gmail.com", "1234", "Sebastian", "Vettel")
                {
                    Id = 2,
                },
                new User("lewis@gmail.com", "1234", "Lewis", "Hamilton")
                {
                    Id = 3,
                },
                new User("max@gmail.com", "1234", "Max", "Verstappen")
                {
                    Id = 4,
                },
                new User("charles@gmail.com", "1234", "Charles", "Leclerc")
                {
                    Id = 5,
                },
                new User("lando@gmail.com", "1234", "Lando", "Norris")
                {
                    Id = 6,
                },
                new User("fernando@gmail.com", "1234", "Fernando", "Alonso")
                {
                    Id = 7,
                },
                new User("george@gmail.com", "1234", "George", "Russell")
                {
                    Id = 8,
                },
                new User("carlos@gmail.com", "1234", "Carlos", "Sainz")
                {
                    Id = 9,
                },
                new User("daniel@gmail.com", "1234", "Daniel", "Ricciardo")
                {
                    Id = 10,
                },
            });

        builder.Entity<Folder>()
            .HasData(new List<Folder>
            {
                new Folder("root", 1, null) { Id = 1 },
                new Folder("root", 2, null) { Id = 2 },
                new Folder("root", 3, null) { Id = 3 },
                new Folder("root", 4, null) { Id = 4 },
                new Folder("root", 5, null) { Id = 5 },
                new Folder("root", 6, null) { Id = 6 },
                new Folder("root", 7, null) { Id = 7 },
                new Folder("root", 8, null) { Id = 8 },
                new Folder("root", 9, null) { Id = 9 },
                new Folder("root", 10, null) { Id = 10 },

                new Folder("Documents", 1, 1) { Id = 11 },
                new Folder("Images", 1, 1) { Id = 12 },
                new Folder("Work", 1, 11) { Id = 13 },
                new Folder("Projects", 1, 11) { Id = 14 },
                new Folder("Screenshots", 1, 12) { Id = 15 },

                new Folder("Work", 2, 2) { Id = 16 },
                new Folder("Personal", 2, 2) { Id = 17 },
                new Folder("Reports", 2, 16) { Id = 18 },
                new Folder("Drafts", 2, 17) { Id = 19 },
                new Folder("Archives", 2, 17) { Id = 20 },

                new Folder("Projects", 3, 3) { Id = 21 },
                new Folder("Backups", 3, 3) { Id = 22 },
                new Folder("Code", 3, 21) { Id = 23 },
                new Folder("Logs", 3, 22) { Id = 24 },
                new Folder("OldProjects", 3, 21) { Id = 25 },

                new Folder("RacingData", 4, 4) { Id = 26 },
                new Folder("Photos", 4, 4) { Id = 27 },
                new Folder("Season2023", 4, 26) { Id = 28 },
                new Folder("Season2022", 4, 26) { Id = 29 },
                new Folder("PodiumPhotos", 4, 27) { Id = 30 },

                new Folder("Videos", 5, 5) { Id = 31 },
                new Folder("Music", 5, 5) { Id = 32 },
                new Folder("Clips", 5, 31) { Id = 33 },
                new Folder("Playlists", 5, 32) { Id = 34 },
                new Folder("Concerts", 5, 32) { Id = 35 },

                new Folder("Downloads", 6, 6) { Id = 36 },
                new Folder("Archives", 6, 6) { Id = 37 },
                new Folder("Compressed", 6, 36) { Id = 38 },
                new Folder("Installers", 6, 36) { Id = 39 },
                new Folder("OldArchives", 6, 37) { Id = 40 },

                new Folder("Games", 7, 7) { Id = 41 },
                new Folder("Settings", 7, 7) { Id = 42 },
                new Folder("SaveFiles", 7, 41) { Id = 43 },
                new Folder("Mods", 7, 41) { Id = 44 },
                new Folder("Config", 7, 42) { Id = 45 },

                new Folder("Simulations", 8, 8) { Id = 46 },
                new Folder("Telemetry", 8, 8) { Id = 47 },
                new Folder("LapTimes", 8, 46) { Id = 48 },
                new Folder("Analytics", 8, 47) { Id = 49 },
                new Folder("CarSetups", 8, 46) { Id = 50 },

                new Folder("Notes", 9, 9) { Id = 51 },
                new Folder("Drafts", 9, 9) { Id = 52 },
                new Folder("Ideas", 9, 51) { Id = 53 },
                new Folder("Plans", 9, 51) { Id = 54 },
                new Folder("Templates", 9, 52) { Id = 55 },

                new Folder("Logs", 10, 10) { Id = 56 },
                new Folder("Temp", 10, 10) { Id = 57 },
                new Folder("ErrorLogs", 10, 56) { Id = 58 },
                new Folder("Cache", 10, 57) { Id = 59 },
                new Folder("SessionLogs", 10, 56) { Id = 60 },
            });

        builder.Entity<File>()
            .HasData(new List<File>
            {
                new File("TodoList", "txt", "Some random text.", 20, 1, 1) { Id = 1 },
                new File("ProjectPlan", "docx", "Some project planning text.", 40, 1, 1) { Id = 2 },

                new File("WorkProposal", "pdf", "Work proposal document.", 50, 1, 13) { Id = 3 },
                new File("TaskList", "txt", "Task list for the week.", 30, 1, 14) { Id = 4 },
                new File("ProfilePicture", "jpg", "Profile picture.", 15, 1, 15) { Id = 5 },

                new File("DesignDoc", "txt", "Design document draft.", 25, 2, 2) { Id = 6 },
                new File("ProjectPlan", "xlsx", "Project financial plan.", 35, 2, 2) { Id = 7 },

                new File("MeetingNotes", "txt", "Meeting notes from last session.", 20, 2, 18) { Id = 8 },
                new File("Report", "pdf", "Financial report for Q1.", 40, 2, 18) { Id = 9 },
                new File("BirthdayGiftIdeas", "txt", "List of gift ideas for birthday.", 15, 2, 19) { Id = 10 },
                new File("VacationPlan", "xlsx", "Vacation plan spreadsheet.", 50, 2, 20) { Id = 11 },

                new File("MeetingAgenda", "txt", "Agenda for the meeting.", 25, 3, 3) { Id = 12 },
                new File("StatusReport", "docx", "Project status report.", 30, 3, 3) { Id = 13 },

                new File("CodeFiles", "zip", "Compressed source code.", 100, 3, 23) { Id = 14 },
                new File("ServerLogs", "txt", "Server log files.", 150, 3, 24) { Id = 15 },

                new File("RaceResults", "csv", "Race results from 2023 season.", 40, 4, 4) { Id = 16 },
                new File("DriverStats", "txt", "Driver performance stats.", 30, 4, 4) { Id = 17 },

                new File("Season2023Results", "xlsx", "Detailed results for 2023.", 80, 4, 28) { Id = 18 },
                new File("Season2022Results", "xlsx", "Detailed results for 2022.", 75, 4, 29) { Id = 19 },
                new File("PodiumPhoto1", "jpg", "Podium photo from the 2023 season.", 20, 4, 30) { Id = 20 },

                new File("MusicTrack1", "mp3", "Track 1 from the album.", 40, 5, 5) { Id = 21 },
                new File("Playlist1", "m3u", "Playlist of favorite tracks.", 10, 5, 5) { Id = 22 },

                new File("MusicVideo1", "mp4", "Official music video.", 100, 5, 33) { Id = 23 },
                new File("Concert1", "mp4", "Concert footage.", 250, 5, 35) { Id = 24 },

                new File("GameInstall", "exe", "Game installation file.", 200, 6, 6) { Id = 25 },
                new File("CompressedFiles", "zip", "Zip file of old projects.", 300, 6, 6) { Id = 26 },

                new File("SoftwareInstaller", "exe", "Software installer package.", 150, 6, 38) { Id = 27 },
                new File("OldArchives", "zip", "Old compressed archives.", 500, 6, 40) { Id = 28 },

                new File("GameSave1", "sav", "Save game file.", 100, 7, 7) { Id = 29 },
                new File("GameSave2", "sav", "Save game file 2.", 120, 7, 7) { Id = 30 },

                new File("GameMods", "zip", "Mods for the game.", 50, 7, 43) { Id = 31 },
                new File("ConfigFile", "txt", "Configuration file for the game.", 15, 7, 45) { Id = 32 },

                new File("LapTimes2023", "txt", "Lap times for 2023 season.", 20, 8, 8) { Id = 33 },
                new File("TelemetryData", "csv", "Telemetry data for season 2023.", 40, 8, 8) { Id = 34 },

                new File("LapTimeAnalysis", "xlsx", "Lap time analysis for 2023.", 60, 8, 49) { Id = 35 },
                new File("CarSetups2023", "txt", "Car setups for season 2023.", 50, 8, 50) { Id = 36 },

                new File("Notes1", "txt", "Random notes for project.", 20, 9, 9) { Id = 37 },
                new File("Ideas", "txt", "Ideas for the new project.", 25, 9, 9) { Id = 38 },

                new File("IdeaDraft", "txt", "Draft of a new idea.", 30, 9, 53) { Id = 39 },
                new File("ProjectPlanDraft", "docx", "Draft of project plan.", 40, 9, 54) { Id = 40 },

                new File("LogFile1", "log", "Log file 1.", 10, 10, 10) { Id = 41 },
                new File("CacheFile", "dat", "Cache data file.", 50, 10, 10) { Id = 42 },

                new File("ErrorLog1", "log", "Error log for system.", 30, 10, 58) { Id = 43 },
                new File("SessionLog1", "log", "Session log file.", 60, 10, 60) { Id = 44 },
            });

        builder.Entity<SharedFolder>()
            .HasData(new List<SharedFolder>
            {
                new SharedFolder(1, 16),
                new SharedFolder(1, 17),
                new SharedFolder(1, 18),
                new SharedFolder(1, 19),
                new SharedFolder(1, 20),
                new SharedFolder(1, 21),
                new SharedFolder(1, 22),
                new SharedFolder(1, 23),
                new SharedFolder(1, 24),
                new SharedFolder(1, 25),

                new SharedFolder(2, 11),
                new SharedFolder(2, 12),
                new SharedFolder(2, 13),
                new SharedFolder(2, 14),
                new SharedFolder(2, 15),
                new SharedFolder(2, 21),
                new SharedFolder(2, 22),
                new SharedFolder(2, 23),
                new SharedFolder(2, 24),
                new SharedFolder(2, 25),
            });

        builder.Entity<SharedFile>()
            .HasData(new List<SharedFile>
            {
                new SharedFile(1, 8),
                new SharedFile(1, 9),
                new SharedFile(1, 10),
                new SharedFile(1, 11),
                new SharedFile(1, 14),
                new SharedFile(1, 15),

                new SharedFile(2, 3),
                new SharedFile(2, 4),
                new SharedFile(2, 5),
                new SharedFile(2, 14),
                new SharedFile(2, 15),
            });

        builder.Entity<FileComment>()
            .HasData(new List<FileComment>
            {
                new FileComment("Some random comment", 1, 1) { Id = 1 },
                new FileComment("Even more random comment", 2, 1) { Id = 2 },
                new FileComment("This is another comment", 3, 2) { Id = 3 },
                new FileComment("A detailed comment", 4, 3) { Id = 4 },
                new FileComment("Comment on the file", 5, 4) { Id = 5 },
                new FileComment("Great file!", 6, 5) { Id = 6 },
                new FileComment("Helpful document", 7, 6) { Id = 7 },
                new FileComment("Awesome project plan", 8, 7) { Id = 8 },
                new FileComment("Interesting design doc", 9, 8) { Id = 9 },
                new FileComment("I like this idea", 10, 9) { Id = 10 },
                new FileComment("Great report", 1, 10) { Id = 11 },
                new FileComment("Very detailed race results", 2, 11) { Id = 12 },
                new FileComment("Noticed a typo", 3, 12) { Id = 13 },
                new FileComment("I agree with this analysis", 4, 13) { Id = 14 },
                new FileComment("This is a useful document", 5, 14) { Id = 15 },
                new FileComment("Needs revision", 6, 15) { Id = 16 },
                new FileComment("Just saved this!", 7, 16) { Id = 17 },
                new FileComment("Great game save!", 8, 17) { Id = 18 },
                new FileComment("Good configuration", 9, 18) { Id = 19 },
                new FileComment("Loving these game mods", 10, 19) { Id = 20 },
                new FileComment("Can’t wait to try this", 1, 20) { Id = 21 },
                new FileComment("Awesome backup!", 2, 21) { Id = 22 },
                new FileComment("Very useful file", 3, 22) { Id = 23 },
                new FileComment("Nice code files", 4, 23) { Id = 24 },
                new FileComment("Logs are clean", 5, 24) { Id = 25 },
                new FileComment("Great analysis", 6, 25) { Id = 26 },
                new FileComment("Perfect telemetry data", 7, 26) { Id = 27 },
                new FileComment("Season 2022 results are helpful", 8, 27) { Id = 28 },
                new FileComment("Old project plans", 9, 28) { Id = 29 },
                new FileComment("Very useful compressed files", 10, 29) { Id = 30 },
                new FileComment("Nice install files", 1, 30) { Id = 31 },
                new FileComment("Game data is great", 2, 31) { Id = 32 },
                new FileComment("This game setup is useful", 3, 32) { Id = 33 },
                new FileComment("Enjoying the music", 4, 33) { Id = 34 },
                new FileComment("Great concert footage", 5, 34) { Id = 35 },
                new FileComment("Fantastic lap times", 6, 35) { Id = 36 },
                new FileComment("Really insightful lap time analysis", 7, 36) { Id = 37 },
                new FileComment("Love the car setups", 8, 37) { Id = 38 },
                new FileComment("Interesting ideas", 9, 38) { Id = 39 },
                new FileComment("Looking forward to this draft", 10, 39) { Id = 40 },
                new FileComment("Very helpful file", 1, 40) { Id = 41 },
                new FileComment("Great for new ideas", 2, 41) { Id = 42 },
                new FileComment("Useful error logs", 3, 42) { Id = 43 },
                new FileComment("Session logs look good", 4, 43) { Id = 44 }
            });
    }
}