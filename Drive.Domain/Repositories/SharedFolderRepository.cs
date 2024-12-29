using Drive.Data.Entities.Models;
using Drive.Data.Entities;
using Drive.Domain.Enums;
using SharedFolder = Drive.Data.Entities.Models.SharedFolder;
using Microsoft.EntityFrameworkCore;

namespace Drive.Domain.Repositories;

public class SharedFolderRepository : BaseRepository
{
    public SharedFolderRepository(DriveDbContext dbContext) : base(dbContext)
    {
    }

    public ResponseResultType Add(SharedFolder folder)
    {
        if (folder == null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.SharedFolders.Add(folder);

        return SaveChanges();
    }

    public ResponseResultType Delete(int id)
    {
        var folderToDelete = DbContext.SharedFolders.Find(id);

        if (folderToDelete is null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.SharedFolders.Remove(folderToDelete);

        return SaveChanges();
    }

    public ICollection<Folder> GetFoldersByUser(User user)
    {
        if (user == null)
        {
            return new List<Folder>();
        }

        var userSharedFolders = DbContext.SharedFolders
                                .Where(f => f.UserId == user.Id)
                                .Include(f => f.Folder)
                                .Where(f => f.Folder != null)
                                .Select(f => f.Folder!)
                                .ToList();

        return userSharedFolders;
    }

    public ICollection<SharedFolder> GetAll() => DbContext.SharedFolders.ToList();
}