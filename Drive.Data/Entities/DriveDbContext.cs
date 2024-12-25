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
            .HasFilter("\"IsRoot\" = TRUE");

        modelBuilder.Entity<Folder>()
            .HasMany(f => f.Subfolders)
            .WithOne(f => f.ParentFolder)
            .HasForeignKey(f => f.ParentFolderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FolderFile>()
            .HasKey(ff => new { ff.FolderId, ff.FileId });

        modelBuilder.Entity<FolderFile>()
            .HasOne(fi => fi.File)
            .WithMany(fi => fi.FolderFiles)
            .HasForeignKey(ff => ff.FileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FolderFile>()
            .HasOne(fo => fo.Folder)
            .WithMany(fo => fo.FolderFiles)
            .HasForeignKey(ff => ff.FolderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<File>()
            .Property(f => f.CreatedOn)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

        modelBuilder.Entity<File>()
            .Property(f => f.LastChanged)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

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