using Drive.Data.Entities.Models;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Extensions;

public static class DiskExtensions
{
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

    public static void PrintDirectoryNavigate(ICollection<Folder> folders, ICollection<File> files, ref int currentIndex)
    {
        if (folders.Count == 0 && files.Count == 0)
        {
            Console.WriteLine("The folder is empty!");
            return;
        }

        var totalCount = folders.Count + files.Count;
        
        if (currentIndex < 0)
            currentIndex = 0;
        else if (currentIndex >= totalCount)
            currentIndex = totalCount - 1;

        if (currentIndex < folders.Count)
        {
            var sortedFolders = folders.OrderBy(f => f.Name).ToList();
            PrintFolders(sortedFolders.Take(currentIndex).ToList());
            Console.Write("\t->");
            PrintFolder(sortedFolders[currentIndex]);
            PrintFolders(sortedFolders.Skip(currentIndex + 1).ToList());
            PrintFiles(files);
        }
        else
        {
            var sortedFiles = files.OrderBy(f => f.LastChanged).ToList();
            PrintFolders(folders);
            PrintFiles(sortedFiles.Take(currentIndex - folders.Count).ToList());
            Console.Write("\t->");
            PrintFile(sortedFiles[currentIndex - folders.Count]);
            PrintFiles(sortedFiles.Skip(currentIndex - folders.Count + 1).ToList());
        }
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

    public static File? GetFileByName(ICollection<File> currentFiles, string fileName, string fileExtension)
    {
        return currentFiles.FirstOrDefault(f => f.Name == fileName && f.Extension == fileExtension);
    }

    public static Folder? GetFolderByName(ICollection<Folder> currentFolders, string folderName)
    {
        return currentFolders.FirstOrDefault(f => f.Name == folderName);
    }
}