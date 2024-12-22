using Drive.Data.Entities.Models;
using Drive.Data.Entities;
using Drive.Domain.Enums;
using File = Drive.Data.Entities.Models.File;
using Microsoft.EntityFrameworkCore;

namespace Drive.Domain.Repositories;

public class FileRepository : BaseRepository
{
    public FileRepository(DriveDbContext dbContext) : base(dbContext)
    {
    }

    public ResponseResultType Add(File file)
    {
        if (file == null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.Files.Add(file);

        return SaveChanges();
    }

    public ResponseResultType Delete(int id)
    {
        var fileToDelete = DbContext.Files.Find(id);

        if (fileToDelete is null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.Files.Remove(fileToDelete);

        return SaveChanges();
    }

    public ResponseResultType Update(File file, int id)
    {
        var fileToUpdate = DbContext.Files.Find(id);

        if (fileToUpdate is null)
        {
            return ResponseResultType.NotFound;
        }

        fileToUpdate.Name = file.Name;
        fileToUpdate.Extension = file.Extension;
        fileToUpdate.Content = file.Content;
        fileToUpdate.Size = file.Size;
        fileToUpdate.LastChanged = DateTime.UtcNow;
        fileToUpdate.FolderFiles = file.FolderFiles;

        return SaveChanges();
    }

    public File? GetById(int id) => DbContext.Files.FirstOrDefault(u => u.Id == id);

    public ICollection<File> GetAllByUser(User user)
    {
        if (user == null)
        {
            return new List<File>();
        }

        var userFiles = DbContext.Files
            .Where(f => f.OwnerId == user.Id)
            .ToList();

        return userFiles;
    }

    public ICollection<File> GetByUser(User user, int currentFolderId)
    {
        if (user == null)
        {
            return new List<File>();
        }

        var filesInFolder = DbContext.Files
        .Where(f => f.OwnerId == user.Id && f.FolderFiles.Any(ff => ff.FolderId == currentFolderId))
        .ToList();

        return filesInFolder;
    }

    public void LoadUsersFiles(User user)
    {
        var userFiles = GetAllByUser(user);

        var userFolderFiles = DbContext.FolderFiles
            .Where(ff => userFiles.Select(f => f.Id).Contains(ff.FileId))
            .ToList();

        foreach (var file in userFiles)
        {
            file.FolderFiles = userFolderFiles.Where(uff => uff.FileId == file.Id).ToList();
            file.Owner = user;
        }
    }

    public ICollection<File> GetAll() => DbContext.Files.ToList();
}