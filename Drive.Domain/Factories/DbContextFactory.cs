using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Drive.Data.Entities;

namespace Drive.Domain.Factories;

public static class DbContextFactory
{
    public static DriveDbContext GetDriveDbContext()
    {
        var options = new DbContextOptionsBuilder()
            .UseNpgsql(ConfigurationManager.ConnectionStrings["Drive"].ConnectionString)
            .Options;

        return new DriveDbContext(options);
    }
}