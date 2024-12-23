using Drive.Data.Entities.Models;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Abstractions;

public interface ICommand
{
    string Name { get; set; }
    string Description { get; set; }
    void Execute(ref Folder currentDirectory, ref ICollection<Folder> currentFolders, ref ICollection<File> currentFiles, string? commandArguments);
}