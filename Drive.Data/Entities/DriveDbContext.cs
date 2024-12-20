using Drive.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Drive.Data.Seeds;

namespace Drive.Data.Entities;

public class DriveDbContext : DbContext
{
    public DriveDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DatabaseSeeder.Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}

public class DriveDbContextFactory : IDesignTimeDbContextFactory<DriveDbContext>
{
    public DriveDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddXmlFile("App.config")
            .Build();

        config.Providers
            .First()
            .TryGet("connectionStrings:add:Drive:connectionString", out var connectionString);

        var options = new DbContextOptionsBuilder<DriveDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new DriveDbContext(options);
    }
}