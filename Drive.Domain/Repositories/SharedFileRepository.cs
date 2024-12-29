using Drive.Data.Entities.Models;
using Drive.Data.Entities;
using Drive.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Domain.Repositories;

public class SharedFileRepository : BaseRepository
{
    public SharedFileRepository(DriveDbContext dbContext) : base(dbContext)
    {
    }

    public ResponseResultType Add(SharedFile file)
    {
        if (file == null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.SharedFiles.Add(file);

        return SaveChanges();
    }

    public ResponseResultType Delete(int id)
    {
        var fileToDelete = DbContext.SharedFiles.Find(id);

        if (fileToDelete is null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.SharedFiles.Remove(fileToDelete);

        return SaveChanges();
    }

    public ICollection<File> GetFilesByUser(User user)
    {
        if (user == null)
        {
            return new List<File>();
        }

        var userSharedFiles = DbContext.SharedFiles
                                .Where(f => f.UserId == user.Id)
                                .Include(f => f.File)
                                .Where(f => f.File != null)
                                .Select(f => f.File!)
                                .ToList();

        return userSharedFiles;
    }

    public ICollection<SharedFile> GetAll() => DbContext.SharedFiles.ToList();
}