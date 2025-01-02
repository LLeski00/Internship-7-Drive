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
    public DbSet<SharedFolder> SharedFolders => Set<SharedFolder>();
    public DbSet<SharedFile> SharedFiles => Set<SharedFile>();
    public DbSet<FileComment> FileComments => Set<FileComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Folder>()
            .HasIndex(f => new { f.OwnerId, f.ParentFolderId })
            .IsUnique()
            .HasFilter("\"ParentFolderId\" IS NULL");

        modelBuilder.Entity<Folder>()
            .HasOne(f => f.ParentFolder)
            .WithMany()
            .HasForeignKey(f => f.ParentFolderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<File>()
            .HasOne(f => f.ParentFolder)
            .WithMany()
            .HasForeignKey(f => f.ParentFolderId)
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

        modelBuilder.Entity<SharedFile>()
            .HasKey(sf => new { sf.FileId, sf.UserId });

        modelBuilder.Entity<SharedFile>()
            .HasOne(sf => sf.File)
            .WithMany(f => f.SharedFiles)
            .HasForeignKey(sf => sf.FileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SharedFile>()
            .HasOne(sf => sf.User)
            .WithMany(u => u.SharedFiles)
            .HasForeignKey(sf => sf.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SharedFolder>()
            .HasKey(sf => new { sf.FolderId, sf.UserId });

        modelBuilder.Entity<SharedFolder>()
            .HasOne(sf => sf.Folder)
            .WithMany(f => f.SharedFolders)
            .HasForeignKey(sf => sf.FolderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SharedFolder>()
            .HasOne(sf => sf.User)
            .WithMany(u => u.SharedFolders)
            .HasForeignKey(sf => sf.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FileComment>()
        .HasOne(fc => fc.File)
        .WithMany(f => f.Comments)
        .HasForeignKey(fc => fc.FileId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FileComment>()
            .HasOne(fc => fc.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(fc => fc.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

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