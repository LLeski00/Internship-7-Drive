using Drive.Data.Entities.Models;
using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
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

    public static bool IsFileNameValid(string file)
    {
        var fileSplitByDot = file.Split('.');

        if (fileSplitByDot.Length != 2 || fileSplitByDot[0].Length == 0 || fileSplitByDot[1].Length == 0)
            return false;

        return true;
    }

    public static bool IsFolderNameValid(string folderName)
    {
        if (string.IsNullOrEmpty(folderName) || folderName.Split('.').Length != 1)
            return false;

        return true;
    }

    public static string InputFileContent()
    {
        var linesOfText = new List<string>();

        do
        {
            var line = Console.ReadLine();

            if (string.IsNullOrEmpty(line))
                break;

            linesOfText.Add(line);
        } while (true);

        var content = string.Join('\n', linesOfText);

        return content;
    }

    public static File? GetFileByName(ICollection<File> currentFiles, string fileName, string fileExtension)
    {
        return currentFiles.FirstOrDefault(f => f.Name == fileName && f.Extension == fileExtension);
    }

    public static Folder? GetFolderByName(ICollection<Folder> currentFolders, string folderName)
    {
        return currentFolders.FirstOrDefault(f => f.Name == folderName);
    }
}