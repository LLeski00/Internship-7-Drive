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
                new User("kimi@gmail.com", "1234", "Kimi", "Raikonen")
                {
                    Id = 1,
                },
                new User("seb@gmail.com", "1234", "Sebastian", "Vettel")
                {
                    Id = 2,
                },
            });

        builder.Entity<File>()
            .HasData(new List<File>
            {
                new File("TodoList", "txt", "Some random text.", 20, 1, 1)
                {
                    Id = 1,
                },
                new File("TodoList2", "txt", "Some random text.", 20, 1, 1)
                {
                    Id = 2,
                },
                new File("TodoList3", "txt", "Some random text.", 20, 1, 2)
                {
                    Id = 3,
                },
                new File("TodoList4", "txt", "Some random text.", 20, 2, 4)
                {
                    Id = 4,
                },
                new File("TodoList5", "txt", "Some random text.", 20, 2, 5)
                {
                    Id = 5,
                },
            });

        builder.Entity<Folder>()
            .HasData(new List<Folder>
            {
                new Folder("root", 1, null)
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
                new Folder("root", 2, null)
                {
                    Id = 4,
                },
                new Folder("new", 2, 4)
                {
                    Id = 5,
                },
                new Folder("some", 2, 4)
                {
                    Id = 6,
                },
            });

        builder.Entity<SharedFile>()
            .HasData(new List<SharedFile>
            {
                new SharedFile(1, 4),
                new SharedFile(2, 1),
            });

        builder.Entity<SharedFolder>()
            .HasData(new List<SharedFolder>
            {
                new SharedFolder(1, 5),
                new SharedFolder(2, 2),
            });
    }
}