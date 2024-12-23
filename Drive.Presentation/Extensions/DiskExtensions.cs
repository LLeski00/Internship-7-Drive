using Drive.Data.Entities.Models;
using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Extensions;

public static class DiskExtensions
{
    public static void LoadUsersDisk(User user)
    {
        var fileRepository = RepositoryFactory.Create<FileRepository>();
        var folderRepository = RepositoryFactory.Create<FolderRepository>();

        fileRepository.LoadUsersFiles(user);
        folderRepository.LoadUsersFolders(user);
    }

    public static void PrintFolder(Folder folder)
    {
        Console.WriteLine($"{folder.Name}");
    }

    public static void PrintFile(File file)
    {
        Console.WriteLine($"{file.Name}.{file.Extension}\tLast changed: {file.LastChanged}");
    }

    public static void PrintFolders(ICollection<Folder> folders)
    {
        var sortedFolders = folders.OrderBy(f => f.Name).ToList();

        foreach (var folder in sortedFolders)
        {
            PrintFolder(folder);
        }
    }

    public static void PrintFiles(ICollection<File> files)
    {
        var sortedFiles = files.OrderBy(f => f.LastChanged).ToList();

        foreach (var file in sortedFiles)
        {
            PrintFile(file);
        }
    }

    public static void PrintDirectory(ICollection<Folder> folders, ICollection<File> files)
    {
        if (folders.Count == 0 && files.Count == 0)
        {
            Console.WriteLine("The folder is empty!");
        }

        PrintFolders(folders);
        PrintFiles(files);
    }
}