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
                new User("llesko00@gmail.com", "1234", "Luka", "Leskovec")
                {
                    Id = 1,
                },
            });

        builder.Entity<File>()
            .HasData(new List<File>
            {
                new File("TodoList", "txt", "Some random text.", 20, 1)
                {
                    Id = 1,
                },
                new File("TodoList2", "txt", "Some random text.", 20, 1)
                {
                    Id = 2,
                },
                new File("TodoList3", "txt", "Some random text.", 20, 1)
                {
                    Id = 3,
                },
                new File("TodoList4", "txt", "Some random text.", 20, 1)
                {
                    Id = 4,
                },
                new File("TodoList5", "txt", "Some random text.", 20, 1)
                {
                    Id = 5,
                },
            });

        builder.Entity<Folder>()
            .HasData(new List<Folder>
            {
                new Folder("root", 1)
                {
                    Id = 1,
                },
                new Folder("obj", 1, 1)
                {
                    Id = 2,
                },
                new Folder("bin", 1, 1)
                {
                    Id = 3,
                },
            });

        builder.Entity<FolderFile>()
            .HasData(new List<FolderFile>
            {
                new FolderFile
                {
                    FolderId = 1,
                    FileId = 1,
                },
                new FolderFile
                {
                    FolderId = 2,
                    FileId = 2,
                },
                new FolderFile
                {
                    FolderId = 3,
                    FileId = 3,
                },
                new FolderFile
                {
                    FolderId = 1,
                    FileId = 4,
                },
                new FolderFile
                {
                    FolderId = 1,
                    FileId = 5,
                },
            });
    }
}