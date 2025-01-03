using Drive.Data.Entities.Models;
using Drive.Data.Entities;
using Drive.Domain.Enums;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Domain.Repositories;

public class FileCommentRepository : BaseRepository
{
    public FileCommentRepository(DriveDbContext dbContext) : base(dbContext)
    {
    }

    public ResponseResultType Add(FileComment fileComment)
    {
        if (fileComment == null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.FileComments.Add(fileComment);

        return SaveChanges();
    }

    public ResponseResultType Delete(int id)
    {
        var fileCommentToDelete = DbContext.FileComments.Find(id);

        if (fileCommentToDelete is null)
        {
            return ResponseResultType.NotFound;
        }

        DbContext.FileComments.Remove(fileCommentToDelete);

        return SaveChanges();
    }

    public ResponseResultType Update(FileComment fileComment, int id)
    {
        var fileCommentToUpdate = DbContext.FileComments.Find(id);

        if (fileCommentToUpdate is null)
        {
            return ResponseResultType.NotFound;
        }

        fileCommentToUpdate.Content = fileComment.Content;
        fileCommentToUpdate.LastChanged = DateTime.UtcNow;

        return SaveChanges();
    }

    public ResponseResultType EditContent(string content, int id)
    {
        var fileCommentToUpdate = DbContext.FileComments.Find(id);

        if (fileCommentToUpdate is null)
        {
            return ResponseResultType.NotFound;
        }

        fileCommentToUpdate.Content = content;

        fileCommentToUpdate.LastChanged = DateTime.UtcNow;

        return SaveChanges();
    }

    public FileComment? GetById(int id) => DbContext.FileComments.FirstOrDefault(u => u.Id == id);

    public ICollection<FileComment> GetByFile(File file)
    {
        if (file == null)
            return new List<FileComment>();

        return DbContext.FileComments.Where(fc => fc.FileId == file.Id).OrderBy(fc => fc.LastChanged).ToList();
    }

    public FileComment? GetByIdInFile(File file, int fileCommentId)
    {
        if (file == null)
            return null;

        return DbContext.FileComments.FirstOrDefault(fc => fc.Id == fileCommentId && fc.FileId == file.Id);
    }

    public ICollection<FileComment> GetAll() => DbContext.FileComments.ToList();
}