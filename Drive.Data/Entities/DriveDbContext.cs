using Drive.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Drive.Data.Seeds;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Data.Entities;

public class DriveDbContext : DbContext
{
    public DriveDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<File> Files => Set<File>();
    public DbSet<Folder> Folders => Set<Folder>();
    public DbSet<FolderFile> FolderFiles => Set<FolderFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Folder>()
            .HasIndex(f => new { f.OwnerId, f.IsRoot })
            .IsUnique()
            .HasFilter("[IsRootFolder] = 1");

        modelBuilder.Entity<FolderFile>()
            .HasKey(ff => new { ff.FolderId, ff.FileId });

        modelBuilder.Entity<FolderFile>()
            .HasOne(fi => fi.File)
            .WithMany(fi => fi.Folders)
            .HasForeignKey(ff => ff.FileId);

        modelBuilder.Entity<FolderFile>()
            .HasOne(fo => fo.Folder)
            .WithMany(fo => fo.Files)
            .HasForeignKey(ff => ff.FolderId);

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