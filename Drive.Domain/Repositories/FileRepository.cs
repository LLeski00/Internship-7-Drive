﻿using Drive.Data.Entities.Models;
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
            return ResponseResultType.NotFound;

        DbContext.Files.Add(file);

        return SaveChanges();
    }

    public ResponseResultType Delete(int id)
    {
        var fileToDelete = DbContext.Files.Find(id);

        if (fileToDelete is null)
            return ResponseResultType.NotFound;

        DbContext.Files.Remove(fileToDelete);

        return SaveChanges();
    }

    public ResponseResultType Update(File file, int id)
    {
        var fileToUpdate = DbContext.Files.Find(id);

        if (fileToUpdate is null)
            return ResponseResultType.NotFound;

        fileToUpdate.Name = file.Name;
        fileToUpdate.Extension = file.Extension;
        fileToUpdate.Content = file.Content;
        fileToUpdate.Size = file.Size;
        fileToUpdate.ParentFolderId = file.ParentFolderId;
        fileToUpdate.LastChanged = DateTime.UtcNow;

        return SaveChanges();
    }

    public ResponseResultType Rename(string fileName, string fileExtension, int id)
    {
        var fileToUpdate = DbContext.Files.Find(id);

        if (fileToUpdate is null)
            return ResponseResultType.NotFound;

        fileToUpdate.Name = fileName;
        fileToUpdate.Extension = fileExtension;
        fileToUpdate.LastChanged = DateTime.UtcNow;

        return SaveChanges();
    }

    public ResponseResultType EditContent(string? content, int id)
    {
        var fileToUpdate = DbContext.Files.Find(id);

        if (fileToUpdate is null)
            return ResponseResultType.NotFound;

        fileToUpdate.Content = content;

        if (content == null)
            fileToUpdate.Size = 0;
        else
            fileToUpdate.Size = content.Length;

        fileToUpdate.LastChanged = DateTime.UtcNow;

        return SaveChanges();
    }

    public File? GetById(int id) => DbContext.Files.FirstOrDefault(u => u.Id == id);

    public ICollection<File> GetAllByUser(User user)
    {
        if (user == null)
            return new List<File>();

        var userFiles = DbContext.Files
            .Where(f => f.OwnerId == user.Id)
            .ToList();

        return userFiles;
    }

    public ICollection<File> GetByUser(User user, int currentFolderId)
    {
        if (user == null)
            return new List<File>();

        var filesInFolder = DbContext.Files
        .Where(f => f.OwnerId == user.Id && f.ParentFolderId == currentFolderId)
        .AsNoTracking()
        .ToList();

        return filesInFolder;
    }

    public ICollection<File> GetByParent(int currentFolderId)
    {
        var filesInFolder = DbContext.Files
        .Where(f => f.ParentFolderId == currentFolderId)
        .ToList();

        return filesInFolder;
    }

    public ICollection<File> GetAll() => DbContext.Files.ToList();
}