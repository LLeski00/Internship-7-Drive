﻿using Drive.Data.Entities.Models;
using Drive.Data.Entities;
using Drive.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using File = Drive.Data.Entities.Models.File;
using Drive.Domain.Factories;

namespace Drive.Domain.Repositories;

public class SharedFileRepository : BaseRepository
{
    public SharedFileRepository(DriveDbContext dbContext) : base(dbContext)
    {
    }

    public ResponseResultType Add(SharedFile file)
    {
        if (file == null)
            return ResponseResultType.NotFound;

        DbContext.SharedFiles.Add(file);

        return SaveChanges();
    }

    public ResponseResultType Delete(int fileId, int userId)
    {
        var fileToDelete = DbContext.SharedFiles.FirstOrDefault(f => f.FileId == fileId && f.UserId == userId);

        if (fileToDelete is null)
            return ResponseResultType.NotFound;

        DbContext.SharedFiles.Remove(fileToDelete);

        return SaveChanges();
    }

    public ICollection<File> GetFilesFromRootByUser(User user)
    {
        if (user == null)
            return new List<File>();

        var _sharedFolderRepository = RepositoryFactory.Create<SharedFolderRepository>();
        var userSharedFolders = _sharedFolderRepository.GetAllFoldersByUser(user)
                                .Select(usf => usf.Id)
                                .ToList();
        var userSharedFiles = DbContext.SharedFiles
                                .Where(f => f.UserId == user.Id && f.File != null)
                                .Include(f => f.File)
                                .Where(f => !userSharedFolders.Contains(f.File!.ParentFolderId))
                                .Select(f => f.File!)
                                .AsNoTracking()
                                .ToList();

        return userSharedFiles;
    }

    public ICollection<File> GetFilesByUser(User user, int parentFolderId)
    {
        if (user == null)
            return new List<File>();

        var _sharedFolderRepository = RepositoryFactory.Create<SharedFolderRepository>();

        if (!_sharedFolderRepository.GetUsersByFolderId(parentFolderId).Any(u => u.Id == user.Id))
            return GetFilesFromRootByUser(user);

        var userSharedFiles = DbContext.SharedFiles
                            .Where(f => f.UserId == user.Id && f.File != null)
                            .Include(f => f.File)
                            .Where(f => f.File!.ParentFolderId == parentFolderId)
                            .Select(f => f.File!)
                            .AsNoTracking()
                            .ToList();

        return userSharedFiles;
    }

    public ICollection<File> GetAllFilesByUser(User user)
    {
        if (user == null)
            return new List<File>();

        var userSharedFiles = DbContext.SharedFiles
                                .Where(f => f.UserId == user.Id)
                                .Include(f => f.File)
                                .Where(f => f.File != null)
                                .Select(f => f.File!)
                                .ToList();

        return userSharedFiles;
    }

    public ICollection<User> GetUsersByFile(File file)
    {
        if (file == null)
            return new List<User>();

        var users = DbContext.SharedFiles
                                .Where(f => f.FileId == file.Id)
                                .Include(f => f.User)
                                .Where(f => f.User != null)
                                .Select(f => f.User!)
                                .ToList();

        return users;
    }

    public ICollection<SharedFile> GetAll() => DbContext.SharedFiles.ToList();
}