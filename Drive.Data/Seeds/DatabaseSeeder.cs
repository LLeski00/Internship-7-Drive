using Drive.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}