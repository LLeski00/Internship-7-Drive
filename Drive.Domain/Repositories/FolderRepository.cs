using Drive.Data.Entities.Models;
using Drive.Data.Entities;
using Drive.Domain.Enums;
using Folder = Drive.Data.Entities.Models.Folder;

namespace Drive.Domain.Repositories;

public class FolderRepository : BaseRepository
{
    public FolderRepository(DriveDbContext dbContext) : base(dbContext)
    {
    }

    public ResponseResultType Add(Folder folder)
    {
        if (folder == null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.Folders.Add(folder);

        return SaveChanges();
    }

    public ResponseResultType Delete(int id)
    {
        var folderToDelete = DbContext.Folders.Find(id);

        if (folderToDelete is null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.Folders.Remove(folderToDelete);

        return SaveChanges();
    }

    public ResponseResultType Update(Folder folder, int id)
    {
        var folderToUpdate = DbContext.Folders.Find(id);

        if (folderToUpdate is null)
        {
            return ResponseResultType.NotFound;
        }

        folderToUpdate.Name = folder.Name;
        folderToUpdate.ParentFolderId = folder.ParentFolderId;
        folderToUpdate.ParentFolder = folder.ParentFolder;
        folderToUpdate.FolderFiles = folder.FolderFiles;
        folderToUpdate.Subfolders = folder.Subfolders;

        return SaveChanges();
    }

    public Folder? GetById(int? id) => DbContext.Folders.FirstOrDefault(u => u.Id == id);

    public ICollection<Folder> GetAllByUser(User user)
    {
        if (user == null)
        {
            return new List<Folder>();
        }

        var userFolders = DbContext.Folders
            .Where(f => f.OwnerId == user.Id)
            .ToList();

        return userFolders;
    }

    public ICollection<Folder> GetByUser(User user, int currentFolderId)
    {
        if (user == null)
        {
            return new List<Folder>();
        }

        var foldersInFolder = DbContext.Folders
            .Where(f => f.OwnerId == user.Id && f.ParentFolderId == currentFolderId)
            .ToList();

        return foldersInFolder;
    }

    public ICollection<Folder> GetByParent(int currentFolderId)
    {
        var foldersInFolder = DbContext.Folders
            .Where(f => f.ParentFolderId == currentFolderId)
            .ToList();

        return foldersInFolder;
    }

    public Folder? GetUsersRoot(User user)
    {
        if (user == null)
        {
            return null;
        }

        var rootFolder = DbContext.Folders
            .FirstOrDefault(f => f.OwnerId == user.Id && f.IsRoot);

        return rootFolder;
    }

    public ICollection<Folder> GetAll() => DbContext.Folders.ToList();

    public void LoadUsersFolders(User user)
    {
        var userFolders = GetAllByUser(user);

        var userFolderFiles = DbContext.FolderFiles
            .Where(ff => userFolders.Select(f => f.Id).Contains(ff.FolderId))
            .ToList();

        foreach (var folder in userFolders)
        {
            folder.ParentFolder = userFolders.FirstOrDefault(uf => uf.Id == folder.ParentFolderId);
            folder.Owner = user;
            folder.FolderFiles = userFolderFiles.Where(uff => uff.FolderId == folder.Id).ToList();
            folder.Subfolders = userFolders.Where(uf => uf.ParentFolderId == folder.Id).ToList();
        }
    }
}