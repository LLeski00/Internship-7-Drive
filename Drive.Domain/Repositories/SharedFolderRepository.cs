using Drive.Data.Entities.Models;
using Drive.Data.Entities;
using Drive.Domain.Enums;
using SharedFolder = Drive.Data.Entities.Models.SharedFolder;
using Microsoft.EntityFrameworkCore;
using Drive.Domain.Factories;

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

    public ResponseResultType Delete(int folderId, int userId)
    {
        var folderToDelete = DbContext.SharedFolders.FirstOrDefault(f => f.FolderId == folderId && f.UserId == userId);

        if (folderToDelete is null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.SharedFolders.Remove(folderToDelete);

        return SaveChanges();
    }

        public ICollection<Folder> GetFoldersFromRootByUser(User user)
    {
        if (user == null)
        {
            return new List<Folder>();
        }

        var userSharedFolders = GetAllFoldersByUser(user)
                                .Select(usf => usf.Id)
                                .ToList();

        var foldersFromRoot = DbContext.SharedFolders
                            .Where(f => f.UserId == user.Id && f.Folder != null)
                            .Include(f => f.Folder)
                            .Where(f => f.Folder!.ParentFolderId == null || !userSharedFolders.Contains(f.Folder!.ParentFolderId.Value))
                            .Select(f => f.Folder!)
                            .ToList();

        return foldersFromRoot;
    }

    public ICollection<Folder> GetFoldersByUser(User user, int parentFolderId)
    {
        if (user == null)
        {
            return new List<Folder>();
        }

        var userSharedfolders = DbContext.SharedFolders
                            .Where(f => f.UserId == user.Id && f.Folder != null)
                            .Include(f => f.Folder)
                            .Where(f => f.Folder!.ParentFolderId == parentFolderId)
                            .Select(f => f.Folder!)
                            .ToList();

        return userSharedfolders;
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

    public ICollection<Folder> GetAllFoldersByUser(User user)
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

    public ICollection<User> GetUsersByFolder(Folder folder)
    {
        if (folder == null)
        {
            return new List<User>();
        }

        var users = DbContext.SharedFolders
                                .Where(f => f.FolderId == folder.Id)
                                .Include(f => f.User)
                                .Where(f => f.User != null)
                                .Select(f => f.User!)
                                .ToList();

        return users;
    }

    public ICollection<SharedFolder> GetAll() => DbContext.SharedFolders.ToList();
}