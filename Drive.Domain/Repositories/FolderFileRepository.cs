using Drive.Data.Entities.Models;
using Drive.Data.Entities;
using Drive.Domain.Enums;

namespace Drive.Domain.Repositories;

public class FolderFileRepository : BaseRepository
{
    public FolderFileRepository(DriveDbContext dbContext) : base(dbContext)
    {
    }

    public ResponseResultType Add(FolderFile folderFile)
    {
        if (folderFile is null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.FolderFiles.Add(folderFile);

        return SaveChanges();
    }

    public ResponseResultType Delete(int id)
    {
        var folderFileToDelete = DbContext.FolderFiles.Find(id);

        if (folderFileToDelete is null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.FolderFiles.Remove(folderFileToDelete);

        return SaveChanges();
    }

    public ResponseResultType Update(FolderFile folderFile, int id)
    {
        var folderFileToUpdate = DbContext.FolderFiles.Find(id);

        if (folderFileToUpdate is null)
        {
            return ResponseResultType.NotFound;
        }

        folderFileToUpdate.FolderId = folderFile.FolderId;
        folderFileToUpdate.Folder = folderFile.Folder;
        folderFileToUpdate.FileId = folderFile.FileId;
        folderFileToUpdate.File = folderFile.File;

        return SaveChanges();
    }

    public ICollection<FolderFile> GetAll() => DbContext.FolderFiles.ToList();
}